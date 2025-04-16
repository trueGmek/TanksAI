using UnityEngine;
using UnityEngine.AI;
using Utils;
using Logger = Utils.Logger;

namespace AI.Core
{
  public class Agent : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navMeshAgent;

    public void Start()
    {
      Logger.Log($"Hello my name is {gameObject.name}", Tags.AGENT);
    }

    public void Move(Vector3 worldPosition)
    {
      if (NavMesh.SamplePosition(worldPosition, out NavMeshHit sampleHit, navMeshAgent.radius, NavMesh.AllAreas))
      {
        navMeshAgent.SetDestination(sampleHit.position);
      }
    }
  }
}