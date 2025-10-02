using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Editor.RemoteControl
{
  public abstract class Payload
  {
  }

  public enum Type
  {
    [EnumMember(Value = "command")] Command,
    [EnumMember(Value = "response")] Response,
    [EnumMember(Value = "log")] Log
  }

  public class Packet
  {
    [JsonProperty("type")] // "command", "response", or "log"
    public Type type { get; set; }

    [JsonProperty("id")] // Unique ID for correlation
    public string id { get; set; }

    [JsonProperty("payload")] // The specific content based on 'type'
    public JObject payload { get; set; }
  }
}
