using System.Text.RegularExpressions;
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

    public LogPayload(string log, string stackTrace, UnityEngine.LogType type)
    {
      level = type.ToString();
      message = log;
      this.stackTrace = stackTrace;
      var fileLinePair = RetrieveFilePath(stackTrace);
      file = fileLinePair.Item1;
      line = fileLinePair.Item2;
    }

    private static (string, int) RetrieveFilePath(string stackTrace)
    {
      const string pattern = @"\(at\s*(?<filepath>.*?):(?<lineNumber>\d+)\)";

      Match match = Regex.Match(stackTrace, pattern);

      if (match.Success == false)
        return (string.Empty, 0);
      
      var filePath = match.Groups["filepath"].Value;
      var lineNumber = match.Groups["lineNumber"].Value;
      return (filePath, System.Int32.Parse(lineNumber));

    }
  }
}
