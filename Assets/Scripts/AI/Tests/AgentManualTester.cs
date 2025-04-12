using AI.Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace AI.Tests
{
    public class AgentManualTester : MonoBehaviour
    {
        [SerializeField] private Agent agent;

        private InputSystem.Actions _inputSystemActions;

        private void Awake()
        {
            _inputSystemActions = new InputSystem.Actions();
            _inputSystemActions.Enable();
            _inputSystemActions.Admin.Interact.performed += OnInteractPerformed;
        }

        private void OnInteractPerformed(InputAction.CallbackContext obj)
        {
            Assert.IsNotNull(Camera.main, "Camera.main != null");
            Ray ray = Camera.main.ScreenPointToRay(_inputSystemActions.Admin.Mouse.ReadValue<Vector2>());
            if (Physics.Raycast(ray, out var hit))
            {
                agent.Move(hit.point);
            }
        }
    }
}