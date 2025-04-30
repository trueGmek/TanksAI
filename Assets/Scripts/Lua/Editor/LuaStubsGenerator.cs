using System;
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
    private const string COMMENT = "---";

    private const BindingFlags BINDING_FLAGS =
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

    static LuaStubsGenerator()
    {
      CompilationPipeline.compilationFinished -= GenerateLuaStubs;
      CompilationPipeline.compilationFinished += GenerateLuaStubs;
    }


    private static void GenerateLuaStubs(object obj)
    {
      Logger.Log("Generating lua stubs", Tags.LUA_CODE_GEN);
      StringBuilder stringBuilder = new();

      stringBuilder.Append(COMMENT).Append(NEW_LINE);
      stringBuilder.Append(COMMENT).Append(SPACE).Append("This file is auto-generated, don't change it!");
      stringBuilder.Append(NEW_LINE).Append(COMMENT).Append(NEW_LINE).Append(NEW_LINE);

      FetchDataFromSourceCode(stringBuilder);

      string path = SaveDataToFile(stringBuilder);
      Logger.Log($"Lua stubs were written to: {path}", Tags.LUA_CODE_GEN);
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

    private static void FetchDataFromSourceCode(StringBuilder stringBuilder)
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

      foreach (Assembly assembly in assemblies)
      {
        if (assembly.FullName.StartsWith("Unity") || assembly.FullName.StartsWith("System"))
          continue;

        foreach (Type type in assembly.GetTypes())
        {
          foreach (MethodInfo method in type.GetMethods(BINDING_FLAGS))
          {
            var attributes = method.GetCustomAttributes(typeof(LuaExportAttribute), false);
            if (attributes.Length <= 0)
            {
              continue;
            }

            foreach (object attribute in attributes)
            {
              EmmitStub(attribute, method, stringBuilder);
              stringBuilder.Append(NEW_LINE);
            }
          }
        }
      }
    }

    private static void EmmitStub(object attribute, MethodInfo method, StringBuilder sb)
    {
      if (attribute is not LuaExportAttribute luaExportAttribute)
        return;

      sb.Append(COMMENT).Append(SPACE).Append(luaExportAttribute.Description).Append(NEW_LINE);
      AppendArgumentsDescriptionComment(method, sb);
      AppendMethodDeclaration(method, sb, luaExportAttribute);
    }

    /// <summary>
    /// Example of what the method generates: 
    /// function log(message) end
    /// </summary>
    private static void AppendMethodDeclaration(MethodInfo method, StringBuilder sb,
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
    /// This is an example of a message that the method generates
    ///  ---@param message string
    /// </summary>
    private static void AppendArgumentsDescriptionComment(MethodInfo method, StringBuilder sb)
    {
      sb.Append(COMMENT).Append(SPACE).Append("@param").Append(SPACE);
      ParameterInfo[] parameter = method.GetParameters();

      foreach (ParameterInfo parameterInfo in parameter)
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

      throw new Exception($"Unknown type: {type.ToString()}");
    }
  }
}