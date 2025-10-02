using Newtonsoft.Json;

namespace Editor.RemoteControl
{
  public class LogPayload : Payload
  {
    [JsonProperty("level")] // "Error", "Warning", "Info", "Exception"
    public string level { get; set; }

    [JsonProperty("message")] // The log content
    public string message { get; set; }

    [JsonProperty("stack_trace")] // Full stack trace (for errors/exceptions)
    public string stackTrace { get; set; }

    [JsonProperty("file")] // Source file path (e.g., "Assets/Scripts/MyScript.cs")
    public string file { get; set; }

    [JsonProperty("line")] // Line number in the source file
    public int line { get; set; }
  }
}
