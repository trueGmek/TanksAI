using UnityEngine;
using Lua;
using Utils;
using UnityEditor;
using AI.Core;

namespace AI.Lua
{
  public class LuaAgentRunner : MonoBehaviour
  {
    [SerializeField, ReadOnly] private string path;
    [SerializeField] private Agent agent;

    private LuaRunner runner;

    private void Awake()
    {
      runner = new LuaRunner();
      var aiBinder = new AiBindings(agent);
      aiBinder.Bind(runner.State);
    }


    private void Start()
    {
      Debug.Log("Starting LOOP AGENT");
      _ = runner.RunPath(path);
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
