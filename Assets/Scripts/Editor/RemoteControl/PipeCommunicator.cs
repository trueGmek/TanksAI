using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Editor.RemoteControl
{
  public class PipeCommunicator
  {
    private const string WRITE_PIPE_NAME = "/tmp/unity-pipe-write";
    private const string READ_PIPE_NAME = "/tmp/unity-pipe-read";

    private Thread writeThread;
    private Thread readThread;
    private NamedPipeServerStream readPipe;
    private NamedPipeServerStream writePipe;
    private ConcurrentQueue<string> writingQueue = new();
    private ConcurrentQueue<string> readingQueue = new();


    public bool IsRunning { get; private set; } = false;
    public readonly CommandHandler Handler = new();

    public void Start()
    {
      writeThread = new Thread(ServerThread_Write);
      writeThread.IsBackground = true;
      writeThread.Start();

      readThread = new Thread(ServerThread_Read);
      readThread.IsBackground = true;
      readThread.Start();

      EditorApplication.update += Update;
      Application.logMessageReceivedThreaded += HandleLog;

      IsRunning = true;
    }

    private void HandleLog(string log, string stackTrace, LogType type)
    {
      writingQueue.Enqueue(CommandHandler.GenerateLogPacket(log, stackTrace, type));
    }

    private void Update()
    {
      var wasSuccessful = readingQueue.TryDequeue(out var result);

      if (wasSuccessful)
      {
        Debug.Log($"Got message from pipe: {result}");
        CommandHandler.Process(result);
      }
    }

    private void ServerThread_Read()
    {
      try
      {
        readPipe = new NamedPipeServerStream(READ_PIPE_NAME, PipeDirection.In);

        Debug.Log($"Waiting for connection on the: {READ_PIPE_NAME}");
        readPipe.WaitForConnection();

        Debug.Log($"Client connected to: {READ_PIPE_NAME}");

        using (var streamReader = new StreamReader(readPipe))
        {
          while (IsRunning)
          {
            var message = streamReader.ReadLine();
            if (message.Length != 0) readingQueue.Enqueue(message);

            Thread.Sleep(100);
          }
        }
      }
      catch //TODO: CATCH EXCEPTION
      {
        IsRunning = false;
      }
      finally
      {
        readPipe.Dispose();
      }
    }

    public void Stop()
    {
      IsRunning = false;
    }

    public void SendMessage(string message)
    {
      writingQueue.Enqueue(message);
    }

    private void ServerThread_Write()
    {
      try
      {
        writePipe = new NamedPipeServerStream(WRITE_PIPE_NAME, PipeDirection.Out);

        Debug.Log($"Waiting for connection on: {WRITE_PIPE_NAME}");
        writePipe.WaitForConnection();
        Debug.Log($"Client connected to: {WRITE_PIPE_NAME}");

        using (var streamWriter = new StreamWriter(writePipe))
        {
          while (IsRunning)
          {
            while (writingQueue.TryDequeue(out var result)) streamWriter.WriteLine(result);

            streamWriter.Flush();
            Thread.Sleep(100);
          }
        }
      }
      catch
      {
        IsRunning = false;
      }
      finally
      {
        writePipe.Dispose();
      }
    }
  }
}
