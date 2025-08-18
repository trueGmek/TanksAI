using System;

namespace Lua
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property)]
  public class LuaExportAttribute : Attribute
  {
    public readonly string LuaName;
    public readonly string Description;

    public LuaExportAttribute(string luaName, string description = null)
    {
      LuaName = luaName;
      Description = description ?? string.Empty;
    }
  }
}
