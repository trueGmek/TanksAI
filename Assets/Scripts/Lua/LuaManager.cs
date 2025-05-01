using Cysharp.Threading.Tasks;
using Lua.Bindings;
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

    public LuaState State { get; private set; }
    private ILuaBinder _genericBindings;

    private void Awake()
    {
      SetUpLuaState();
    }

    private void SetUpLuaState()
    {
      Logger.Log("Setting up LUA state", Tags.LUA_MANAGER);
      State = LuaState.Create();
      State.OpenStandardLibraries();

      _genericBindings = new GenericBindings();

      _genericBindings.Bind(State);
    }


    private async UniTask RunScript()
    {
      Assert.IsNotNull(State);
      Logger.Log($"Running a script! \n{script.Text}", Tags.LUA_MANAGER);

      try
      {
        await State.DoStringAsync(script.Text);
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