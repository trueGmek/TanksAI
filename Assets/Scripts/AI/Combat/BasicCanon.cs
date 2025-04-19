using UnityEngine;
using Logger = Utils.Logger;
using Tags = Utils.Tags;

namespace AI.Combat
{
  public class BasicCanon : Canon
  {
    [SerializeField] private Animator animator;
    

    private static readonly int ShootTrigger = Animator.StringToHash("shoot");

    public override void Shoot(Vector3 direction)
    {
      Logger.Log("Shooting", Tags.CANON);
      animator.SetTrigger(ShootTrigger);
    }
  }
}