using UnityEngine;
using Lua;
namespace Lua
{
  public static class TypeUtils
  {
    public static Vector3 GetVector3(this LuaTable luaTable)
    {
      float x = luaTable["X"].Read<float>();
      float y = luaTable["Y"].Read<float>();
      float z = luaTable["Z"].Read<float>();

      return new Vector3(x, y, z);
    }
  }
}
