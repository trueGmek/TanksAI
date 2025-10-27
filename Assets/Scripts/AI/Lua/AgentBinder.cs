using System;
using System.Threading;
using System.Threading.Tasks;
using AI.Core;
using Lua;
using Lua.Bindings;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Logger = Utils.Logger;

namespace AI.Lua
{
  [LuaExport(AGENT_CLASS_NAME)]
  public class AgentBinder : ILuaBinder
  {
    public const string AGENT_CLASS_NAME = "Agent";
    public const string MOVE_METHOD_NAME = "move";
    public const string ROTATE_METHOD_NAME = "rotate";
    public const string SHOOT_METHOD_NAME = "shoot";

    private readonly Agent _agent;
    private readonly LuaFunction[] _functions;

    public AgentBinder(Agent agent)
    {
      _agent = agent;
      _functions = new LuaFunction[]
      {
        new(MOVE_METHOD_NAME, MoveBinding),
        new(ROTATE_METHOD_NAME, RotateBinding),
        new(SHOOT_METHOD_NAME, ShootBinding),
      };
    }

    public void Bind(in LuaState state)
    {
      Assert.IsNotNull(state);

      LuaTable agent = new(0, _functions.Length);
      foreach (LuaFunction func in _functions)
      {
        agent[func.Name] = func;
      }

      state.Environment[AGENT_CLASS_NAME] = agent;
    }

    private ValueTask<int> ShootBinding(LuaFunctionExecutionContext context, Memory<LuaValue> memory, CancellationToken cToken)
    {
      LuaTable args = context.GetArgument<LuaTable>(1);

      Shoot(args.GetVector3());

      return new ValueTask<int>(0);
    }

    [LuaExport(SHOOT_METHOD_NAME, "Shots the canon at the given position")]
    private void Shoot(Vector3 worldPosition)
    {
      Logger.Log($"Executing shoot through lua on again: {_agent.name}", Tags.AGENT);
      _agent.Shoot((worldPosition - _agent.transform.position).normalized);
    }

    private ValueTask<int> RotateBinding(LuaFunctionExecutionContext arg1, Memory<LuaValue> arg2,
      CancellationToken arg3)
    {
      Rotate();
      return new ValueTask<int>(0);
    }

    [LuaExport(ROTATE_METHOD_NAME, "Rotates the cannon on the tank to point to a specified direction")]
    private void Rotate()
    {
      Logger.Log($"Executing rotate through lua on again: {_agent.name}", Tags.AGENT);
      _agent.Rotate(_agent.transform.forward);
    }


    private ValueTask<int> MoveBinding(LuaFunctionExecutionContext context, Memory<LuaValue> memory,
      CancellationToken ctoken)
    {
      LuaTable args = context.GetArgument<LuaTable>(1);

      Move(args.GetVector3());

      return new ValueTask<int>(0);
    }

    [LuaExport(MOVE_METHOD_NAME, "Moves the agent to a world position")]
    private void Move(Vector3 direction)
    {
      _agent.Move(_agent.transform.position + _agent.transform.TransformDirection(direction));
    }
  }
}
