using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Utils;
using Assembly = System.Reflection.Assembly;
using Logger = Utils.Logger;

namespace Lua.Editor
{
  [InitializeOnLoad]
  public static class LuaStubsGenerator
  {
    private const char NEW_LINE = '\n';
    private const char SPACE = ' ';
    private const char COMMA = ',';
    private const char OPEN_BRACKET = '(';
    private const char CLOSE_BRACKET = ')';
    private const char AT = '@';
    private const string COMMENT = "---";

    private const BindingFlags FUNCTIONS_BINDING_FLAGS =
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    private const BindingFlags METHODS_BINDING_FLAGS =
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    private const BindingFlags PROPERTIES_BINDING_FLAGS =
      BindingFlags.Public | BindingFlags.Instance;

    private static Stopwatch _stopwatch = new Stopwatch();

    static LuaStubsGenerator()
    {
      CompilationPipeline.compilationFinished -= GenerateLuaStubs;
      CompilationPipeline.compilationFinished += GenerateLuaStubs;
    }


    private static void GenerateLuaStubs(object obj)
    {
      Logger.Log("Generating lua stubs", Tags.LUA_CODE_GEN);
      _stopwatch.Start();

      StringBuilder stringBuilder = new();

      stringBuilder.Append(COMMENT).Append(NEW_LINE);
      stringBuilder.Append(COMMENT).Append(SPACE).Append("This file is auto-generated, don't change it!");
      stringBuilder.Append(NEW_LINE).Append(COMMENT).Append(NEW_LINE).Append(NEW_LINE);

      FetchData(stringBuilder);

      string path = SaveDataToFile(stringBuilder);

      _stopwatch.Stop();
      Logger.Log(
        $"Generation finished! It took: {_stopwatch.Elapsed.TotalSeconds} seconds. Lua stubs were written to: {path}",
        Tags.LUA_CODE_GEN);
    }

    private static void FetchData(StringBuilder stringBuilder)
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

      foreach (Assembly assembly in assemblies)
      {
        if (assembly.FullName.StartsWith("Unity") || assembly.FullName.StartsWith("System"))
          continue;

        foreach (Type type in assembly.GetTypes())
        {
          GenerateStubsForClasses(stringBuilder, type);
          GenerateStubsForMethods(stringBuilder, type);
          GenerateStubsForFunctions(stringBuilder, type);
        }
      }
    }

    private static void GenerateStubsForClasses(StringBuilder stringBuilder, Type type)
    {
      LuaExportAttribute luaExportAttribute = GetLuaAttributeFromType(type);

      if (luaExportAttribute == null)
        return;

      stringBuilder.Append(COMMENT).Append(AT).Append("class").Append(SPACE).Append(luaExportAttribute.LuaName)
        .Append(NEW_LINE);
      GenerateStubsForProperties(stringBuilder, type);
      stringBuilder.Append(NEW_LINE);
    }

    private static void GenerateStubsForProperties(StringBuilder stringBuilder, Type type)
    {
      foreach (PropertyInfo property in type.GetProperties(PROPERTIES_BINDING_FLAGS))
      {
        object[] attributes = property.GetCustomAttributes(typeof(LuaExportAttribute), false);
        if (attributes.Length <= 0)
        {
          continue;
        }

        foreach (object attribute in attributes)
        {
          if (attribute is not LuaExportAttribute luaExportAttribute)
            return;
          AppendPropertyInfo(property, stringBuilder);
          stringBuilder.Append(NEW_LINE);
        }
      }
    }

    private static void GenerateStubsForMethods(StringBuilder stringBuilder, Type type)
    {
      foreach (MethodInfo method in type.GetMethods(METHODS_BINDING_FLAGS))
      {
        object[] attributes = method.GetCustomAttributes(typeof(LuaExportAttribute), false);
        if (attributes.Length <= 0)
        {
          continue;
        }

        foreach (object attribute in attributes)
        {
          if (attribute is not LuaExportAttribute luaExportAttribute)
            return;

          stringBuilder.Append(COMMENT).Append(SPACE).Append(luaExportAttribute.Description).Append(NEW_LINE);
          AppendArgumentsDescriptionComment(method, stringBuilder);
          AppendMethodDeclaration(method, stringBuilder, luaExportAttribute);
          stringBuilder.Append(NEW_LINE);
        }
      }
    }

    private static void GenerateStubsForFunctions(StringBuilder stringBuilder, Type type)
    {
      foreach (MethodInfo method in type.GetMethods(FUNCTIONS_BINDING_FLAGS))
      {
        object[] attributes = method.GetCustomAttributes(typeof(LuaExportAttribute), false);
        if (attributes.Length <= 0)
        {
          continue;
        }

        foreach (object attribute in attributes)
        {
          if (attribute is not LuaExportAttribute luaExportAttribute)
            return;

          stringBuilder.Append(COMMENT).Append(SPACE).Append(luaExportAttribute.Description).Append(NEW_LINE);

          AppendArgumentsDescriptionComment(method, stringBuilder);
          AppendFunctionDeclaration(method, stringBuilder, luaExportAttribute);
          stringBuilder.Append(NEW_LINE);
        }
      }
    }

    private static string SaveDataToFile(StringBuilder stringBuilder)
    {
      string path = Path.Combine(Application.dataPath, "Lua/Stubs/CSharpAPI.g.lua");

      if (File.Exists(path))
        File.Delete(path);

      using StreamWriter streamWriter = new(path);
      streamWriter.Write(stringBuilder.ToString());
      streamWriter.Close();
      File.SetAttributes(path, FileAttributes.ReadOnly);

      return path;
    }

    /// <summary>
    /// Example of what the method generates: 
    /// function log(message) end
    /// </summary>
    private static void AppendFunctionDeclaration(MethodInfo method, StringBuilder sb,
      LuaExportAttribute luaExportAttribute)
    {
      sb.Append("function").Append(SPACE).Append(luaExportAttribute.LuaName);

      sb.Append(OPEN_BRACKET);

      ParameterInfo[] parameter = method.GetParameters();

      for (int i = 0; i < parameter.Length; i++)
      {
        ParameterInfo parameterInfo = parameter[i];
        if (i > 1)
        {
          sb.Append(COMMA).Append(SPACE);
        }

        sb.Append(parameterInfo.Name);
      }

      sb.Append(CLOSE_BRACKET);
      sb.Append(SPACE).Append("end").Append(NEW_LINE);
    }

    /// <summary>
    /// Example of what the method generates: 
    /// function log(message) end
    /// </summary>
    private static void AppendMethodDeclaration(MethodInfo method, StringBuilder sb,
      LuaExportAttribute luaExportAttribute)
    {
      LuaExportAttribute classLuaExportAttribute = GetLuaAttributeFromType(method.DeclaringType);

      if (classLuaExportAttribute == null)
      {
        Logger.LogError($"Could not find a parent lua attribute for method: {method.Name}", Tags.LUA_CODE_GEN);
        return;
      }

      sb.Append("function").Append(SPACE).Append(classLuaExportAttribute.LuaName).Append(':')
        .Append(luaExportAttribute.LuaName);
      sb.Append(OPEN_BRACKET);

      ParameterInfo[] parameter = method.GetParameters();

      for (int i = 0; i < parameter.Length; i++)
      {
        ParameterInfo parameterInfo = parameter[i];
        if (i > 1)
        {
          sb.Append(COMMA).Append(SPACE);
        }

        sb.Append(parameterInfo.Name);
      }

      sb.Append(CLOSE_BRACKET);
      sb.Append(SPACE).Append("end").Append(NEW_LINE);
    }

    private static LuaExportAttribute GetLuaAttributeFromType(Type type)
    {
      if (type == null)
        return null;

      IEnumerable<Attribute> declaringTypeAttributes = type.GetCustomAttributes(typeof(LuaExportAttribute));
      LuaExportAttribute classLuaExportAttribute = null;

      foreach (Attribute attribute in declaringTypeAttributes)
      {
        if (attribute is not LuaExportAttribute classLuaAttribute)
        {
          continue;
        }

        classLuaExportAttribute = classLuaAttribute;
      }

      return classLuaExportAttribute;
    }

    /// <summary>
    /// This is an example of a message that the method generates
    ///  ---@field message string
    /// </summary>
    private static void AppendPropertyInfo(PropertyInfo property, StringBuilder sb)
    {
      sb.Append(COMMENT).Append("@field").Append(SPACE);
      sb.Append(property.Name).Append(SPACE);
      sb.Append(property.PropertyType.ToLuaType()).Append(SPACE);
    }

    /// <summary>
    /// This is an example of a message that the method generates
    ///  ---@param message string
    /// </summary>
    private static void AppendArgumentsDescriptionComment(MethodInfo method, StringBuilder sb)
    {
      ParameterInfo[] parameters = method.GetParameters();
      if (parameters.Length == 0)
        return;

      sb.Append(COMMENT).Append(SPACE).Append("@param").Append(SPACE);

      foreach (ParameterInfo parameterInfo in parameters)
      {
        sb.Append(parameterInfo.Name).Append(SPACE);
        sb.Append(parameterInfo.ParameterType.ToLuaType()).Append(SPACE);
      }

      sb.Append(NEW_LINE);
    }

    private static string ToLuaType(this Type type)
    {
      if (type == null)
      {
        return "nil";
      }

      if (type == typeof(bool))
      {
        return "bool";
      }

      if (type == typeof(string))
      {
        return "string";
      }

      if (type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(decimal))
      {
        return "number";
      }

      if (type == typeof(UnityEngine.Vector3))
      {
        return "Vector3";
      }

      throw new Exception($"Unknown type: {type}");
    }
  }
}
