using AI.Combat;
using AI.Utils;
using UnityEngine.AI;

namespace AI.Core
{
  public interface IBlackboard
  {
    public Canon Canon { get; }
    public BasicCanonData CanonData { get; }
    public NavMeshAgent NavMeshAgent { get; }
    public VisualDebugger VisualDebugger { get; }
  }
}