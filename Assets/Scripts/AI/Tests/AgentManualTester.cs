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
      _inputSystemActions.Admin.Select.performed += ManageSelectAction;
      _inputSystemActions.Admin.Interact.performed += ManageInteractAction;
    }
    
    public void Bind(params Agent[] agents)
    {
      Assert.IsNotNull(agents, "agents == null");
      Assert.IsFalse(agents.Length == 0, "agents.Length == 0");

      Logger.Log($"Binding {agents.Length} agents", Tags.MANUAL_TESTER);

      _agents = new List<Agent>();
      _agents.AddRange(agents);
    }

    private void ManageInteractAction(InputAction.CallbackContext obj)
    {
      if (TryCastRayFromCamera(out RaycastHit hit) == false)
        return;
      
      Agent hitAgent = hit.transform.GetComponentInParent<Agent>();

      if (hitAgent != null && _currentAgent != hitAgent)
      {
        Shoot(hitAgent);
        return;
      }

      if (_currentAgent != null)
        MoveCurrentAgent(hit);
    }

    private void Shoot(Agent hitAgent)
    {
      Logger.Log($"Agent {_currentAgent.name} is shooting", Tags.MANUAL_TESTER);
      var shootDir = (hitAgent.transform.position - _currentAgent.transform.position).normalized;
      Debug.DrawLine(_currentAgent.transform.position, hitAgent.transform.position, Color.red, 1f);
      // _currentAgent.Rotate(shootDir);
      _currentAgent.Shoot(shootDir);
    }

    private void ManageSelectAction(InputAction.CallbackContext obj)
    {
      if (TryCastRayFromCamera(out RaycastHit hit) == false)
        return;

      TryBindAgent(hit);
    }

    private bool TryCastRayFromCamera(out RaycastHit hit)
    {
      Assert.IsNotNull(Camera.main, "Camera.main != null");
      Ray ray = Camera.main.ScreenPointToRay(_inputSystemActions.Admin.Mouse.ReadValue<Vector2>());

      return Physics.Raycast(ray, out hit);
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