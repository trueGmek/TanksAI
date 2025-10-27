using UnityEngine;
using Lua;
using Utils;
using AI.Core;
using Cysharp.Threading.Tasks;

namespace AI.Lua
{
  public class LuaAgentRunner : MonoBehaviour
  {
    [SerializeField, ReadOnly] private string path;
    [SerializeField] private Agent agent;

    public LuaRunner Runner { get; private set; }
    private LuaValue[] result;

    private Agent opponent;

    public void Initialize()
    {
      Runner = new LuaRunner();

      var aiBinder = new AgentBinder(agent);
      aiBinder.Bind(Runner.State);

      var opponentBinder = new OpponentBinder(opponent);
      opponentBinder.Bind(Runner.State);
    }

    public void StartAgent()
    {
      _ = StartAgentExecution();
    }

    internal void SetOpponent(Agent opponent)
    {
      this.opponent = opponent;
    }

    private async UniTask StartAgentExecution()
    {
      result = await Runner.RunPath(path);

      var functionObject = result[0].Read<LuaTable>();
      var start = functionObject["Start"].Read<LuaFunction>();
      var update = functionObject["Update"].Read<LuaFunction>();

      await start.InvokeAsync(Runner.State, new LuaValue[] { Time.deltaTime });
      while (enabled)
      {
        await UniTask.WaitForFixedUpdate();
        await update.InvokeAsync(Runner.State, new LuaValue[] { Time.deltaTime });
      }
    }


#if UNITY_EDITOR
    [Button("Select file")]
    private void SelectFile()
    {
      path = UnityEditor.EditorUtility.OpenFilePanelWithFilters("Select LUA script", "Assets/Lua/",
        new string[] { "Lua Files", "lua", "All files", "*" });
      UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
  }
}
