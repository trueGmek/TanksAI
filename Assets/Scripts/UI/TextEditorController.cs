using System;
using System.IO;
using Lua;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TextEditorController : MonoBehaviour
{
  [SerializeField] private Button playButton;
  [SerializeField] private Button loadButton;
  [SerializeField] private Button saveButton;
  [SerializeField] private TMP_InputField inputField;

  [SerializeField] private LuaManager luaManager;

  private string filepath;
  private string buffer;

  private void Awake()
  {
    loadButton.onClick.AddListener(OpenFile);
    playButton.onClick.AddListener(RunScript);
    saveButton.onClick.AddListener(SaveFile);
  }

  private void SaveFile()
  {
    File.WriteAllText(filepath, inputField.text);
  }

  private void RunScript()
  {
    _ = luaManager.Run(inputField.text);
  }

  public void OpenFile()
  {
    filepath = EditorUtility.OpenFilePanelWithFilters("Select LUA script", "",
      new string[] { "Lua Files", "lua", "All files", "*" });
    buffer = File.ReadAllText(filepath);
    inputField.SetTextWithoutNotify(buffer);
  }
}
