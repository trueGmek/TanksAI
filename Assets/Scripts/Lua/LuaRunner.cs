using System.IO;
using Cysharp.Threading.Tasks;
using Lua.Bindings;
using Lua.Standard;
using UnityEngine.Assertions;
using Utils;
using Logger = Utils.Logger;

namespace Lua
{
  public class LuaRunner
  {
    public LuaState State { get; private set; }

    private ILuaBinder _genericBindings;

    public LuaRunner()
    {
      Logger.Log("Setting up LUA state", Tags.LUA_MANAGER);
      State = LuaState.Create();
      State.OpenStandardLibraries();

      _genericBindings = new GenericBindings();
      _genericBindings.Bind(State);
    }

    public async UniTask<LuaValue[]> RunPath(string filepath)
    {
      string script = await File.ReadAllTextAsync(filepath);
      return await Run(script);
    }


    public async UniTask<LuaValue[]> Run(string script)
    {
      Assert.IsNotNull(State);

      Logger.Log($"Running a script! \n```\n{script}\n```", Tags.LUA_MANAGER);

      try
      {
        var results = await State.DoStringAsync(script);
        Logger.Log("Script finished!", Tags.LUA_MANAGER);
        return results;
      }
      catch (LuaParseException e)
      {
        Logger.LogError($"A parse exception was thrown: {e}", Tags.LUA_MANAGER);
        return null;
      }
      catch (LuaRuntimeException e)
      {
        Logger.LogError($"Runtime error: {e}", Tags.LUA_MANAGER);
        return null;
      }
      catch (System.Exception e)
      {
        Logger.LogError($"Runtime error: {e}", Tags.LUA_MANAGER);
        return null;
      }
    }
  }
}
