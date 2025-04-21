using System;
using AI.Combat;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Core
{
  public class Agent : MonoBehaviour
  {
    [SerializeField] private Blackboard blackboard;

    public Blackboard Blackboard => blackboard;
    private NavMeshAgent NavMeshAgent => Blackboard.NavMeshAgent;
    private Canon Canon => Blackboard.Canon;

    public void Move(Vector3 worldPosition)
    {
      var navMeshAgent = blackboard.NavMeshAgent;

      if (NavMesh.SamplePosition(worldPosition, out NavMeshHit sampleHit, navMeshAgent.radius, NavMesh.AllAreas))
      {
        navMeshAgent.SetDestination(sampleHit.position);
      }
    }

    public void Rotate(Vector3 direction)
    {
      NavMeshAgent.updateRotation = false;
      transform.LookAt(direction);
      NavMeshAgent.updateRotation = true;
    }
      
    public void Shoot(Vector3 direction)
    {
      Canon.Shoot(direction);
    }
  }
}