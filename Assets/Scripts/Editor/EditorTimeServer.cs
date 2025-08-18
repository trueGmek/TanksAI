using System;
using UnityEditor;
using UnityEngine;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;

[InitializeOnLoad]
public static class EditorTimeServer
{
  private const string REGENERATE_ENDPOINT = "/regenerate-solution";
  private const string GET_LOGS_ENDPOINT = "/get-logs";
  private const string PORT_NUMBER = "5005";
  private static readonly string UriPrefix = $"http://localhost:{PORT_NUMBER}/";

  private static HttpListener _listener;
  private static Thread _listenerThread;

  private static readonly StringBuilder StringBuilder = new StringBuilder();
  private static readonly Queue<Action> QueuedActions = new Queue<Action>();

  static EditorTimeServer()
  {
    StartServer();
  }

  private static void StartServer()
  {
    _listener = new HttpListener();

    _listener.Prefixes.Add(UriPrefix);
    _listenerThread = new Thread(ListenLoop);
    _listenerThread.IsBackground = true;
    _listenerThread.Start();
    Debug.Log("BackgroundRegenerateServer running at http://localhost:5005/");

    EditorApplication.update += ProcessQueuedActions;

    Application.logMessageReceivedThreaded += HandleLog;
  }


  private static void ListenLoop()
  {
    _listener.Start();
    while (true)
    {
      try
      {
        var context = _listener.GetContext();
        var requestPath = context.Request.Url.AbsolutePath;

        if (requestPath == REGENERATE_ENDPOINT)
        {
          lock (QueuedActions)
          {
            QueuedActions.Enqueue(() =>
            {
              StringBuilder.Clear();
              AssetDatabase.Refresh();
              Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();

              Debug.Log("✅ Project files regenerated via HTTP request (background-safe)");
            });
          }

          Respond(context, 200, "Regeneration triggered");
        }
        else if (requestPath == GET_LOGS_ENDPOINT)
        {
          Respond(context, 200, StringBuilder.ToString());
        }
        else
        {
          Respond(context, 404, "Not Found");
        }
      }
      catch
      {
        Debug.Log("Stopping the thread");
        break;
      }
    }
  }

  private static void ProcessQueuedActions()
  {
    lock (QueuedActions)
    {
      while (QueuedActions.Count > 0)
      {
        QueuedActions.Dequeue()?.Invoke();
      }
    }
  }

  private static void HandleLog(string logString, string stackTrace, LogType type)
  {
    StringBuilder.Append("[").Append(type).Append("]");
    StringBuilder.Append("[").Append(DateTime.Now.ToString(CultureInfo.InvariantCulture)).Append("]");
    StringBuilder.Append(": ").Append(logString).Append('\n');
  }

  private static void Respond(HttpListenerContext context, int status, string message)
  {
    context.Response.StatusCode = status;
    var buffer = Encoding.UTF8.GetBytes(message);
    context.Response.ContentLength64 = buffer.Length;
    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    context.Response.OutputStream.Close();
  }
}
