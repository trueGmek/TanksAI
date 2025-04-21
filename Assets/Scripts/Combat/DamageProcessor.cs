using UnityEngine;

namespace Combat
{
  public abstract class DamageProcessor : MonoBehaviour
  {
    public abstract void ApplyDamage(DamageData damageData);
  }
}