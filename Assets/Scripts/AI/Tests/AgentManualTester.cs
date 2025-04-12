using System.Collections.Generic;
using AI.Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

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
      Assert.IsNull(agents, "agents == null");
      Assert.IsFalse(agents.Length == 0, "agents.Length == 0");

      _agents.Clear();
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
      Agent hitAgent = hit.transform.GetComponent<Agent>();
      if (hitAgent != null)
      {
        _currentAgent = hitAgent;
        return;
      }

      if (_currentAgent != null)
        MoveCurrentAgent(hit);
    }

    private void MoveCurrentAgent(RaycastHit hitInfo)
    {
      Assert.IsNotNull(_currentAgent, "_currentAgent != null");

      _currentAgent.Move(hitInfo.point);
    }
  }
}