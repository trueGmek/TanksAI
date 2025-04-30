using Cysharp.Threading.Tasks;
using Lua.Standard;
using Lua.Unity;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Logger = Utils.Logger;

namespace Lua
{
  public class LuaManager : MonoBehaviour
  {
    [SerializeField] private LuaAsset script;

    private LuaState _state;

    private void Start()
    {
      Logger.Log("Starting the manager", Tags.LUA_MANAGER);
      _state = LuaState.Create();
      _state.OpenStandardLibraries();
      _state.AddProjectMethods();
      RunScript().Forget();
    }

    private async UniTask RunScript()
    {
      Assert.IsNotNull(_state);
      Logger.Log($"Running a script! \n{script.Text}", Tags.LUA_MANAGER);

      try
      {
        await _state.DoStringAsync(script.Text);
      }
      catch (LuaParseException e)
      {
        Logger.LogError($"A parse exception was thrown: {e}", Tags.LUA_MANAGER);
        return;
      }
      catch (LuaRuntimeException e)
      {
        Logger.LogError(e, Tags.LUA_MANAGER);
        return;
      }

      Logger.Log("Script finished!", Tags.LUA_MANAGER);
    }

    [Button("Run the script")]
    private void RunTheScript()
    {
      RunScript().Forget();
    }
  }
}