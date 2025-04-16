using System;
using UnityEngine;

namespace Utils
{
  public static class Logger
  {
    public static void Log(string message, string tag = "")
    {
      Debug.Log(FormatMessage(message, tag));
    }


    public static void LogWarning(string message, string tag = "")
    {
      Debug.LogWarning(FormatMessage(message, tag));
    }

    public static void LogError(string message, string tag = "")
    {
      Debug.LogError(FormatMessage(message, tag));
    }

    private static string FormatMessage(string message, string tag)
    {
      var tagBit = string.Empty;
      if (tag != string.Empty)
        tagBit = $" [{tag}]";

      return $"{DateTime.Now:s}{tagBit}: {message}";
    }
  }
}