using AI.Core;
using Combat;
using UnityEngine;
using Utils;
using Logger = Utils.Logger;

namespace AI.Combat
{
  public class TankDamageProcessor : DamageProcessor, ISystem
  {
    private static readonly int GetHIt = Animator.StringToHash("GetHit");

    private Animator _animator;
    private Health _health;

    public void Initialize(IBlackboard blackboard)
    {
      _animator = blackboard.Animator;
      _health = blackboard.Health;
    }

    public override void ApplyDamage(DamageData damageData)
    {
      Logger.Log($"Tank: {gameObject.name} received damage!", Tags.AGENT);
      _animator.SetTrigger(GetHIt);
      _health.Decrease(damageData.Damage);
    }
  }
}