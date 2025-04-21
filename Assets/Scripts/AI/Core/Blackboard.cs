using AI.Combat;
using AI.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Core
{
  public class Blackboard : MonoBehaviour, IBlackboard
  {
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private VisualDebugger visualDebugger;
    [SerializeField] private Canon canon;
    [SerializeField] private BasicCanonData canonData;

    public Canon Canon => canon;
    public BasicCanonData CanonData => canonData;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public VisualDebugger VisualDebugger => visualDebugger;
  }
}