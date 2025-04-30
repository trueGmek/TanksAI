using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Lua
{
  public static class LuaExtensions
  {
    private const string LOG_NAME = "log";

    public static void AddProjectMethods(this LuaState state)
    {
      Assert.IsNotNull(state, "State cannot be null");
      state.Environment[LOG_NAME] = new LuaFunction(Log);
    }

    private static ValueTask<int> Log(LuaFunctionExecutionContext context, Memory<LuaValue> buffer,
      CancellationToken cancellationToken)
    {
      string arg0 = context.GetArgument<string>(0);

      LogCore(arg0);

      return new ValueTask<int>(1);
    }


    [LuaExport(LOG_NAME, "Logs a message through the internal logger")]
    private static void LogCore(string message)
    {
      Assert.IsNotNull(message, "Argument from LUA cannot be null");
      Utils.Logger.Log(message, Utils.Tags.LUA);
    }
  }
}