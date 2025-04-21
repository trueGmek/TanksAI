using Combat;
using UnityEngine;

namespace AI.Combat
{
  public class ProjectileDamageApplier : DamageApplier
  {
    [SerializeField] private float damage;

    protected override DamageData CreateDamageData()
    {
      return new DamageData(damage);
    }
  }
}