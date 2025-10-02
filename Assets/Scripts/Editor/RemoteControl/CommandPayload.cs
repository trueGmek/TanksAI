using System.Collections.Generic;
using Newtonsoft.Json;

namespace Editor.RemoteControl
{
  public enum Action
  {
    Recompile,
    RunTest
  }

  public class CommandPayload : Payload
  {
    [JsonProperty("action")] public Action action { get; set; }

    [JsonProperty("arguments")] public Dictionary<string, object> arguments { get; set; } = new();
  }

  public static class RecompileCommandResolver
  {
    public static void Resolve(CommandPayload commandPayload)
    {
      UnityEditor.AssetDatabase.Refresh();
      Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
    }
  }
}
