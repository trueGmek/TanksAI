using UnityEngine;
using UnityEngine.AI;

namespace AI.Core
{
  public class Agent : MonoBehaviour
  {
    [SerializeField] private Blackboard blackboard;

    public Blackboard Blackboard => blackboard;

    public void Move(Vector3 worldPosition)
    {
      var navMeshAgent = blackboard.NavMeshAgent;

      if (NavMesh.SamplePosition(worldPosition, out NavMeshHit sampleHit, navMeshAgent.radius, NavMesh.AllAreas))
      {
        navMeshAgent.SetDestination(sampleHit.position);
      }
    }
  }
}