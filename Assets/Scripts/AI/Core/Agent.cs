using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Core
{
  public class Agent : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent navMeshAgent;

    public void Start()
    {
      Debug.Log($"Hello my name is {gameObject.name}");
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