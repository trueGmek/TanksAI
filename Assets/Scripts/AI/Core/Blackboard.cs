using AI.Combat;
using AI.Utils;
using Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace AI.Core
{
  public class Blackboard : MonoBehaviour, IBlackboard
  {
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private VisualDebugger visualDebugger;
    [SerializeField] private Canon canon;
    [SerializeField] private BasicCanonData canonData;
    [SerializeField] private TankDamageProcessor damageProcessor;
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;
    [SerializeField] private Agent agent;

    private void Awake()
    {
      Assert.IsNotNull(canon, "Canon != null");
      Assert.IsNotNull(canonData, "CanonData != null");
      Assert.IsNotNull(navMeshAgent, "NavMeshAgent != null");
      Assert.IsNotNull(visualDebugger, "VisualDebugger != null");
      Assert.IsNotNull(damageProcessor, "DamageProcessor != null");
      Assert.IsNotNull(animator, "Animator != null");
      Assert.IsNotNull(health, "health != null");
      Assert.IsNotNull(agent, "agent != null");
    }

    private void Start()
    {
      canon.Initialize(this);
      damageProcessor.Initialize(this);
      agent.Initialize(this);
    }

    public Canon Canon => canon;
    public BasicCanonData CanonData => canonData;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public VisualDebugger VisualDebugger => visualDebugger;
    public DamageProcessor DamageProcessor => damageProcessor;
    public Animator Animator => animator;
    public Health Health => health;
    public Agent Agent => agent;
  }
}