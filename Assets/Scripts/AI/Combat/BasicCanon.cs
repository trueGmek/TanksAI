using AI.Core;
using UnityEngine;
using Logger = Utils.Logger;
using Tags = Utils.Tags;

namespace AI.Combat
{
  public class BasicCanon : Canon
  {
    private BasicCanonData _canonData;
    private static readonly int ShootTrigger = Animator.StringToHash("shoot");

    public override void Initialize(IBlackboard blackboard)
    {
      _canonData = blackboard.CanonData;
    }

    public override void Shoot(Vector3 direction)
    {
      Logger.Log("Shooting", Tags.CANON);
      _canonData.Animator.SetTrigger(ShootTrigger);

      Transform spawnPoint = _canonData.SpawnPoint;
      Rigidbody projectile = Instantiate(_canonData.Projectile, spawnPoint.position, spawnPoint.rotation);
      projectile.AddForce(direction * _canonData.Velocity, ForceMode.Acceleration);
    }
  }
}