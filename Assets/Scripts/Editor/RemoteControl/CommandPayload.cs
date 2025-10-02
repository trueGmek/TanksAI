using System.Collections.Generic;
using Newtonsoft.Json;

namespace Editor.RemoteControl
{
  public class CommandPayload : Payload
  {
    [JsonProperty("action")] // e.g., "Recompile", "RunTest"
    public string action { get; set; }

    [JsonProperty("arguments")] // Using Dictionary<string, object> for flexible key-value arguments
    public Dictionary<string, object> arguments { get; set; } = new Dictionary<string, object>();
  }
}
