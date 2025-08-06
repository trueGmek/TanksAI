using System.IO;
using Cysharp.Threading.Tasks;
using Lua.Bindings;
using Lua.Standard;
using Lua.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Logger = Utils.Logger;

namespace Lua
{
  public class LuaManager : MonoBehaviour
  {
    [ReadOnly, SerializeField] private string filepath;
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
      string script = await File.ReadAllTextAsync(filepath);
      await Run(script);
    }

    public async UniTask Run(string script)
    {
      Assert.IsNotNull(State);

      Logger.Log($"Running a script! \n```\n{script}\n```", Tags.LUA_MANAGER);

      try
      {
        await State.DoStringAsync(script);
      }
      catch (LuaParseException e)
      {
        Logger.LogError($"A parse exception was thrown: {e}", Tags.LUA_MANAGER);
        return;
      }
      catch (LuaRuntimeException e)
      {
        Logger.LogError($"Runtime error: {e}", Tags.LUA_MANAGER);
        return;
      }

      Logger.Log("Script finished!", Tags.LUA_MANAGER);
    }


    [Button("Run the script")]
    private void RunTheScript()
    {
      RunScript().Forget();
    }

    [Button("Select file")]
    private void SelectFile()
    {
      filepath = EditorUtility.OpenFilePanelWithFilters("Select LUA script", "",
        new string[] { "Lua Files", "lua", "All files", "*" });
    }
  }
}
