using System.Collections.Generic;
using AI.Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Utils;
using Logger = Utils.Logger;

namespace AI.Tests
{
  public class AgentManualTester : MonoBehaviour
  {
    private InputSystem.Actions _inputSystemActions;

    private List<Agent> _agents;
    private Agent _currentAgent;

    private void Awake()
    {
      _inputSystemActions = new InputSystem.Actions();
      _inputSystemActions.Enable();
      _inputSystemActions.Admin.Interact.performed += ManageClickAction;
    }

    public void Bind(params Agent[] agents)
    {
      Assert.IsNotNull(agents, "agents == null");
      Assert.IsFalse(agents.Length == 0, "agents.Length == 0");

      Logger.Log($"Binding {agents.Length} agents", Tags.MANUAL_TESTER);

      _agents = new List<Agent>();
      _agents.AddRange(agents);
    }

    private void ManageClickAction(InputAction.CallbackContext obj)
    {
      Assert.IsNotNull(Camera.main, "Camera.main != null");
      Ray ray = Camera.main.ScreenPointToRay(_inputSystemActions.Admin.Mouse.ReadValue<Vector2>());

      bool wasHit = Physics.Raycast(ray, out var hit);
      if (wasHit == false)
        return;

      PerformAction(hit);
    }

    private void PerformAction(RaycastHit hit)
    {
      if (TryBindAgent(hit))
        return;

      if (_currentAgent != null)
        MoveCurrentAgent(hit);
    }

    private bool TryBindAgent(RaycastHit hit)
    {
      Agent hitAgent = hit.transform.GetComponentInParent<Agent>();

      if (hitAgent == null)
        return false;

      Logger.Log($"Binding agent: {hitAgent.gameObject.name}", Tags.MANUAL_TESTER);

      _currentAgent?.Blackboard.VisualDebugger.ResetColor();
      _currentAgent = hitAgent;
      _currentAgent.Blackboard.VisualDebugger.SetColor(Color.red);
      return true;
    }

    private void MoveCurrentAgent(RaycastHit hitInfo)
    {
      Assert.IsNotNull(_currentAgent, "_currentAgent != null");

      Logger.Log($"Moving agent to new position: {_currentAgent.gameObject.name}", Tags.MANUAL_TESTER);
      _currentAgent.Move(hitInfo.point);
    }
  }
}