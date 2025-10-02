using System.Collections.Generic;
using Newtonsoft.Json;

namespace Editor.RemoteControl
{
  public class ResponsePayload : Payload
  {
    [JsonProperty("status")] 
    public string status { get; set; }

    [JsonProperty("message")] 
    public string message { get; set; }

    [JsonProperty("data")] 
    public Dictionary<string, object> data { get; set; } = new();
  }
}
