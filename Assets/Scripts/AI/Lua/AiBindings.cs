using System;
using System.Threading;
using System.Threading.Tasks;
using AI.Core;
using Lua;
using Lua.Bindings;
using UnityEngine.Assertions;
using Utils;

namespace AI.Lua
{
  public class AiBindings : ILuaBinder
  {
    private readonly Agent _agent;
    private readonly LuaFunction[] _functions;

    public AiBindings(Agent agent)
    {
      _agent = agent;
      _functions = new LuaFunction[]
      {
        new("move", MoveInternal),
        new("rotate", RotateInternal),
        new("shoot", ShootInternal),
      };
    }

    public void Bind(in LuaState state)
    {
      Assert.IsNotNull(state);
      Logger.Log($"Binding agent: {_agent.name} to lua state", Tags.AGENT);

      LuaTable agent = new(0, _functions.Length);
      foreach (LuaFunction func in _functions)
      {
        agent[func.Name] = func;
      }

      state.Environment["agent"] = agent;
    }

    private ValueTask<int> ShootInternal(LuaFunctionExecutionContext arg1, Memory<LuaValue> arg2,
      CancellationToken arg3)
    {
      Logger.Log($"Executing shoot through lua on again: {_agent.name}", Tags.AGENT);
      _agent.Shoot(_agent.transform.forward);
      return new ValueTask<int>(0);
    }

    private ValueTask<int> RotateInternal(LuaFunctionExecutionContext arg1, Memory<LuaValue> arg2,
      CancellationToken arg3)
    {
      Logger.Log($"Executing rotate through lua on again: {_agent.name}", Tags.AGENT);
      _agent.Rotate(_agent.transform.forward);
      return new ValueTask<int>(0);
    }


    private ValueTask<int> MoveInternal(LuaFunctionExecutionContext arg1, Memory<LuaValue> arg2,
      CancellationToken arg3)
    {
      Logger.Log($"Executing move through lua on again: {_agent.name}", Tags.AGENT);
      _agent.Move(_agent.transform.forward);
      return new ValueTask<int>(0);
    }
  }
}