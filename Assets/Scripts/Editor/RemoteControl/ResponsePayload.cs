using System.Collections.Generic;
using Newtonsoft.Json;

namespace Editor.RemoteControl
{
  public class ResponsePayload : Payload
  {
    [JsonProperty("status")] // "Success" or "Error"
    public string status { get; set; }

    [JsonProperty("message")] // Human-readable result
    public string message { get; set; }

    [JsonProperty("data")] // For optional metadata like compilation duration, test results, etc.
    public Dictionary<string, object> data { get; set; } = new Dictionary<string, object>();
  }
}
