using AI.Core;
using UnityEngine;

namespace AI.Combat
{
  public abstract class Canon : MonoBehaviour, ISystem
  {
    public abstract void Shoot(Vector3 direction);
    public abstract void Initialize(IBlackboard blackboard);
  }
}