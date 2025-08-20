using System;
using System.Threading;
using System.Threading.Tasks;
using AI.Core;
using Lua;
using Lua.Bindings;
using UnityEngine;
using UnityEngine.Assertions;

namespace AI.Lua
{
  [LuaExport(CLASS_NAME, "Class for information about the opponent")]
  public class OpponentBinder : ILuaBinder
  {
    public const string CLASS_NAME = "Opponent";
    public const string GET_POSITION_NAME = "GetPosition";

    private readonly Agent opponent;

    public OpponentBinder(Agent opponent)
    {
      this.opponent = opponent;
    }

    public void Bind(in LuaState state)
    {
      Assert.IsNotNull(state);
      LuaFunction[] functions = new LuaFunction[] { new(GET_POSITION_NAME, GetPositionBinding) };

      LuaTable opponentTable = new(0, functions.Length);
      foreach (LuaFunction func in functions)
      {
        opponentTable[func.Name] = func;
      }

      state.Environment[CLASS_NAME] = opponentTable;
    }

    private ValueTask<int> GetPositionBinding(LuaFunctionExecutionContext context, Memory<LuaValue> memory,
      CancellationToken token)
    {
      UnityEngine.Vector3 position = GetPosition();
      var result = new LuaTable
      {
        ["X"] = new LuaValue(position.x),
        ["Y"] = new LuaValue(position.y),
        ["Z"] = new LuaValue(position.z)
      };
      memory.Span[0] = new LuaValue(result);

      return new ValueTask<int>(1);
    }

    [LuaExport(GET_POSITION_NAME, "Returns opponent position")]
    private Vector3 GetPosition()
    {
      return opponent.transform.position;
    }
  }
}
