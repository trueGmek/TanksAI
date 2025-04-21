using AI.Core;
using UnityEngine;
using Utils;
using Logger = Utils.Logger;

namespace AI.Combat
{
  public class RotatingCanon : Canon
  {
    private BasicCanonData _canonData;
    private static readonly int ShootTrigger = Animator.StringToHash("shoot");

    public override void Initialize(IBlackboard blackboard)
    {
      _canonData = blackboard.CanonData;
    }

    public override void Shoot(Vector3 direction)
    {
      transform.LookAt(direction);
      Logger.Log("Shooting", Tags.CANON);
      _canonData.Animator.SetTrigger(ShootTrigger);

      Transform spawnPoint = _canonData.SpawnPoint;
      Rigidbody projectile = Instantiate(_canonData.Projectile, spawnPoint.position, spawnPoint.rotation);
      projectile.AddForce(direction * _canonData.Velocity, ForceMode.Acceleration);
    }
  }
}