using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Editor.RemoteControl
{
    public class CommandHandler
    {
        public CommandHandler() { }

        public void Process(string data)
        {
            Packet packet = JsonConvert.DeserializeObject<Packet>(data);
            if (packet == null)
            {
                Debug.LogError($"Couldn't deserialize packet: {data}");
                return;
            }

            if (packet.type != Type.Command)
                return;

            var command = JsonConvert.DeserializeObject<CommandPayload>(packet.payload.ToString());
            if (command.action != "Recompile")
                return;

            UnityEditor.AssetDatabase.Refresh();
            Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
            UnityEngine.Debug.Log("✅ Project files regenerated via HTTP request (background-safe)");
        }

        public string GenerateLogPacket(string log, string stackTrace, LogType type)
        {
            Packet packet = new Packet();
            packet.id = "test";
            packet.type = Type.Log;
            var payload = new LogPayload(log, stackTrace, type);
            packet.payload = JObject.FromObject(payload);
            return JsonConvert.SerializeObject(packet, new JsonSerializerSettings { Formatting = Formatting.Indented, Converters = { new StringEnumConverter() } });
        }
    }
}
