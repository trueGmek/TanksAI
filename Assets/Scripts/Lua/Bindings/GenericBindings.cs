using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace Lua.Bindings
{
  public class GenericBindings : ILuaBinder
  {
    private const string LOG = "Log";
    private const string WAIT = "Wait";

    public void Bind(in LuaState state)
    {
      Assert.IsNotNull(state, "State cannot be null");
      state.Environment[LOG] = new LuaFunction(LOG, Log);
      state.Environment[WAIT] = new LuaFunction(WAIT, Wait);
    }


    private static async ValueTask<int> Wait(LuaFunctionExecutionContext context, Memory<LuaValue> buffer,
      CancellationToken cancellationToken)
    {
      float arg0 = context.GetArgument<float>(0);

      await WaitInternal(arg0);

      return 0;
    }

    [LuaExport(WAIT, "Delays the execution for number of milliseconds")]
    private static async UniTask WaitInternal(float milliseconds)
    {
      await UniTask.Delay(TimeSpan.FromMilliseconds(milliseconds));
    }

    private static ValueTask<int> Log(LuaFunctionExecutionContext context, Memory<LuaValue> buffer,
      CancellationToken cancellationToken)
    {
      string arg0 = context.GetArgument<string>(0);

      LogCore(arg0);

      return new ValueTask<int>(0);
    }


    [LuaExport(LOG, "Logs a message through the internal logger")]
    private static void LogCore(string message)
    {
      Assert.IsNotNull(message, "Argument from LUA cannot be null");
      Utils.Logger.Log(message, Utils.Tags.LUA);
    }
  }
}