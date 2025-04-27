using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Lua
{
  public static class LuaExtensions
  {
    public static void AddProjectMethods(this LuaState state)
    {
      Assert.IsNotNull(state, "State cannot be null");
      state.Environment["log"] = new LuaFunction(Log);
    }

    private static ValueTask<int> Log(LuaFunctionExecutionContext context, Memory<LuaValue> buffer,
      CancellationToken cancellationToken)
    {
      string arg0 = context.GetArgument<string>(0);
      
      Assert.IsNotNull(arg0, "Argument from LUA cannot be null");
      
      Utils.Logger.Log(arg0, Utils.Tags.LUA);
      return new ValueTask<int>(1);
    }
  }
}