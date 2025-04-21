using System;
using UnityEngine;
using Utils;

namespace Combat
{
  public abstract class DamageApplier : MonoBehaviour
  {
    private void OnTriggerEnter(Collider other)
    {
      Utils.Logger.Log($"DamageApplier entered trigger with {other.name}", Tags.DAMAGE_APPLIER);

      DamageProcessor damageProcessor = other.GetComponent<DamageProcessor>();
      if (damageProcessor == null)
        return;

      try
      {
        damageProcessor.ApplyDamage(CreateDamageData());
      }
      catch (Exception e)
      {
        Utils.Logger.LogError(e, Tags.DAMAGE_APPLIER);
      }
      finally
      {
        Destroy(gameObject);
      }
    }

    protected abstract DamageData CreateDamageData();
  }
}