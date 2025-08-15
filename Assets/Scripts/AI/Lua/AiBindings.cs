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
  [LuaExport(AGENT_CLASS_NAME)]
  public class AiBindings : ILuaBinder
  {
    public const string AGENT_CLASS_NAME = "Agent";
    public const string MOVE_METHOD_NAME = "move";
    public const string ROTATE_METHOD_NAME = "rotate";
    public const string SHOOT_METHOD_NAME = "shoot";

    private readonly Agent _agent;
    private readonly LuaFunction[] _functions;

    public AiBindings(Agent agent)
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
      Logger.Log($"Binding agent: {_agent.name} to lua state", Tags.AGENT);

      LuaTable agent = new(0, _functions.Length);
      foreach (LuaFunction func in _functions)
      {
        agent[func.Name] = func;
      }

      state.Environment[AGENT_CLASS_NAME] = agent;
    }

    private ValueTask<int> ShootBinding(LuaFunctionExecutionContext arg1, Memory<LuaValue> arg2,
      CancellationToken arg3)
    {
      Shoot();
      return new ValueTask<int>(0);
    }

    [LuaExport(SHOOT_METHOD_NAME, "Shoots the canon in the forward direction of the canon")]
    private void Shoot()
    {
      Logger.Log($"Executing shoot through lua on again: {_agent.name}", Tags.AGENT);
      _agent.Shoot(_agent.transform.forward);
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


    private ValueTask<int> MoveBinding(LuaFunctionExecutionContext arg1, Memory<LuaValue> arg2, CancellationToken arg3)
    {
      Move();
      return new ValueTask<int>(0);
    }

    [LuaExport(MOVE_METHOD_NAME, "Moves the agent to a world position")]
    private void Move()
    {
      Logger.Log($"Executing move through lua on again: {_agent.name}", Tags.AGENT);
      _agent.Move(_agent.transform.position + _agent.transform.forward);
    }
  }
}
