using UnityEditor;
using UnityEngine;

namespace Editor.RemoteControl
{
  public class NeovimCommunicatorEditor : EditorWindow
  {
    private PipeCommunicator pipeCommunicator;

    private void OnEnable()
    {
      pipeCommunicator = new PipeCommunicator();
    }

    [MenuItem("Tools/Neovim Communicator")]
    public static void ShowWindow()
    {
      GetWindow<NeovimCommunicatorEditor>("Neovim Communicator");
    }

    private void OnGUI()
    {
      GUILayout.Label("Neovim Connection", EditorStyles.boldLabel);

      if (pipeCommunicator.IsRunning == false)
      {
        if (GUILayout.Button("Start the server"))
        {
          pipeCommunicator.Start();
        }
      }
      else
      {
        if (GUILayout.Button("Kill the server"))
        {
          pipeCommunicator.Stop();
        }

        if (GUILayout.Button("Send Test Message to Neovim"))
        {
          pipeCommunicator.SendMessage("TEST MESSAGE");
        }
      }
    }

    private void OnDestroy()
    {
      pipeCommunicator.Stop();
    }
  }
}
