using UnityEngine;
using UnityEditor;
using Utils;
using Cysharp.Threading.Tasks;

namespace Lua
{
  public class StandaloneLuaRunner : MonoBehaviour
  {
    [SerializeField, ReadOnly] private string filepath;

    public LuaRunner Runner { get; private set; }

    private void Awake()
    {
      Runner = new LuaRunner();
    }

    [Button("Run the script")]
    private void RunTheScript()
    {
      Runner.RunPath(filepath).Forget();
    }

    [Button("Select file")]
    private void SelectFile()
    {
      filepath = EditorUtility.OpenFilePanelWithFilters("Select LUA script", "",
        new string[] { "Lua Files", "lua", "All files", "*" });
    }
  }
}
