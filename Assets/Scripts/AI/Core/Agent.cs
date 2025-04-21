using AI.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Core
{
  public class Agent : MonoBehaviour, ISystem
  {
    public IBlackboard Blackboard { get; private set; }
    private NavMeshAgent NavMeshAgent => Blackboard.NavMeshAgent;
    private Canon Canon => Blackboard.Canon;

    public void Initialize(IBlackboard blackboard)
    {
      Blackboard = blackboard;
      Blackboard.Health.OnDeath += HandleAgentDeath;
    }

    public void Move(Vector3 worldPosition)
    {
      var navMeshAgent = Blackboard.NavMeshAgent;

      if (NavMesh.SamplePosition(worldPosition, out NavMeshHit sampleHit, navMeshAgent.radius, NavMesh.AllAreas))
      {
        navMeshAgent.SetDestination(sampleHit.position);
      }
    }

    //TODO: MANAGE ROTATION
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

    private void HandleAgentDeath()
    {
      Destroy(gameObject);
    }
  }
}