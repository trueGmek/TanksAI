using UnityEngine;
using Lua;
using Utils;
using UnityEditor;
using AI.Core;
using Cysharp.Threading.Tasks;

namespace AI.Lua
{
  public class LuaAgentRunner : MonoBehaviour
  {
    [SerializeField, ReadOnly] private string path;
    [SerializeField] private Agent agent;

    private LuaRunner runner;
    private LuaValue[] result;

    private void Awake()
    {
      runner = new LuaRunner();
      var aiBinder = new AiBindings(agent);
      aiBinder.Bind(runner.State);

      _ = StartAgentExecution();
    }

    private async UniTask StartAgentExecution()
    {
      result = await runner.RunPath(path);

      var functionObject = result[0].Read<LuaTable>();
      var start = functionObject["Start"].Read<LuaFunction>();
      var update = functionObject["Update"].Read<LuaFunction>();

      await start.InvokeAsync(runner.State, new LuaValue[] { Time.deltaTime });
      while (enabled)
      {
        await UniTask.WaitForFixedUpdate();
        await update.InvokeAsync(runner.State, new LuaValue[] { Time.deltaTime });
      }
    }

    [Button("Select file")]
    private void SelectFile()
    {
      path = EditorUtility.OpenFilePanelWithFilters("Select LUA script", "",
        new string[] { "Lua Files", "lua", "All files", "*" });
      EditorUtility.SetDirty(this);
    }
  }
}
