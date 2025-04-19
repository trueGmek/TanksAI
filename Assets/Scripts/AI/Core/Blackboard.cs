using AI.Combat;
using AI.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Core
{
  public class Blackboard : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private VisualDebugger visualDebugger;
    [SerializeField] private Canon canon;

    public Canon Canon => canon;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public VisualDebugger VisualDebugger => visualDebugger;
  }
}