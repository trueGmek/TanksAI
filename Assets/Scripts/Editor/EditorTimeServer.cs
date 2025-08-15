using UnityEditor;
using UnityEngine;
using System.Net;
using System.Text;
using System.Threading;

[InitializeOnLoad]
public static class EditorTimeServer
{
  private static HttpListener listener;
  private static Thread listenerThread;

  static EditorTimeServer()
  {
    StartServer();
  }

  private static void StartServer()
  {
    listener = new HttpListener();
    listener.Prefixes.Add("http://localhost:5005/");
    listenerThread = new Thread(ListenLoop);
    listenerThread.IsBackground = true;
    listenerThread.Start();
    Debug.Log("BackgroundRegenerateServer running at http://localhost:5005/");

    // Ensure editor callbacks are processed even when unfocused
    EditorApplication.update += ProcessQueuedActions;
  }

  private static readonly System.Collections.Generic.Queue<System.Action> queuedActions =
      new System.Collections.Generic.Queue<System.Action>();

  private static void ListenLoop()
  {
    listener.Start();
    while (true)
    {
      try
      {
        var context = listener.GetContext();
        var requestPath = context.Request.Url.AbsolutePath;

        if (requestPath == "/regenerate-solution")
        {
          lock (queuedActions)
          {
            queuedActions.Enqueue(() =>
            {
              AssetDatabase.Refresh();
              Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
              Debug.Log("✅ Project files regenerated via HTTP request (background-safe)");
            });
          }

          Respond(context, 200, "Regeneration triggered");
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
    lock (queuedActions)
    {
      while (queuedActions.Count > 0)
      {
        queuedActions.Dequeue()?.Invoke();
      }
    }
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
