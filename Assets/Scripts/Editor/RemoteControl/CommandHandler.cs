using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System;

namespace Editor.RemoteControl
{
  public class CommandHandler
  {
    private readonly string projectName = UnityEditor.PlayerSettings.productName;
    private const string TIMESTAMP_FORMAT = "yyyy-MM-dd-HH:mm:ss";

    public string GenerateCommandId()
    {
      var timestamp = DateTime.Now.ToString(TIMESTAMP_FORMAT);
      var rand = UnityEngine.Random.Range(1, 1001);

      return $"{projectName}_{timestamp}_{rand}";
    }

    public static void Process(string data)
    {
      var packet = JsonConvert.DeserializeObject<Packet>(data);
      if (packet == null)
      {
        Debug.LogError($"Couldn't deserialize packet: {data}");
        return;
      }

      if (packet.type != Type.Command)
        return;

      var command = JsonConvert.DeserializeObject<CommandPayload>(packet.payload.ToString());
      if (command.action == Action.Recompile) RecompileCommandResolver.Resolve(command);
    }

    public static string GenerateLogPacket(string log, string stackTrace, LogType type)
    {
      var payload = new LogPayload(log, stackTrace, type);
      var packet = new Packet
      {
        id = "test",
        type = Type.Log,
        payload = JObject.FromObject(payload)
      };
      return JsonConvert.SerializeObject(packet,
        new JsonSerializerSettings { Formatting = Formatting.Indented, Converters = { new StringEnumConverter() } });
    }
  }
}
