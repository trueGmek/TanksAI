using UnityEngine;

namespace Lua
{
  [LuaExport("Vector3")]
  public partial class LuaVector3
  {
    Vector3 vector;

    [LuaExport("x")]
    public float X
    {
      get => vector.x;
      set => vector = new Vector3(value, vector.y, vector.z);
    }

    [LuaExport("x")]
    public float Y
    {
      get => vector.y;
      set => vector = new Vector3(vector.x, value, vector.z);
    }

    [LuaExport("x")]
    public float Z
    {
      get => vector.z;
      set => vector = new Vector3(vector.x, vector.y, value);
    }

    // [LuaExport("create", "Creates new instance of a vector")]
    // public static LuaVector3 Create(float x, float y, float z)
    // {
    //   return new LuaVector3()
    //   {
    //     vector = new Vector3(x, y, z)
    //   };
    // }

    // [LuaExport("normalized", "Normalizes the vector")]
    // public LuaVector3 Normalized()
    // {
    //   return new LuaVector3()
    //   {
    //     vector = Vector3.Normalize(vector)
    //   };
    // }
  }
}
