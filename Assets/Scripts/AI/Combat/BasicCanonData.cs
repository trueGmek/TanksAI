using System;
using UnityEngine;

namespace AI.Combat
{
  [Serializable]
  public class BasicCanonData
  {
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody projectile;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float velocity;

    public Animator Animator => animator;
    public Rigidbody Projectile => projectile;
    public Transform SpawnPoint => spawnPoint;
    public float Velocity => velocity;
  }
}