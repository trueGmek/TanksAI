using System;
using AI.Combat;
using AI.Utils;
using Combat;
using NUnit.Framework;
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
    [SerializeField] private TankDamageProcessor damageProcessor;
    [SerializeField] private Animator animator;

    private void Awake()
    {
      Assert.IsNotNull(Canon, "Canon != null");
      Assert.IsNotNull(CanonData, "CanonData != null");
      Assert.IsNotNull(NavMeshAgent, "NavMeshAgent != null");
      Assert.IsNotNull(VisualDebugger, "VisualDebugger != null");
      Assert.IsNotNull(DamageProcessor, "DamageProcessor != null");
      Assert.IsNotNull(Animator, "Animator != null");
      
      canon.Initialize(this);
      damageProcessor.Initialize(this);
    }

    public Canon Canon => canon;
    public BasicCanonData CanonData => canonData;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public VisualDebugger VisualDebugger => visualDebugger;
    public DamageProcessor DamageProcessor => damageProcessor;
    public Animator Animator => animator;
  }
}