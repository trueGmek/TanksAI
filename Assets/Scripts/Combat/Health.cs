using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Combat
{
  public class Health : MonoBehaviour
  {
    [SerializeField] private float maxHealth;
    
    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsAlive => CurrentHealth > 0;

    public Action OnDeath;

    private void Awake()
    {
      CurrentHealth = maxHealth;
    }

    public void Decrease(float value)
    {
      Assert.IsTrue(IsAlive, "Cannot decrease health of a dead instance");
      CurrentHealth -= value;

      if (CurrentHealth <= 0)
        OnDeath?.Invoke();
    }

    public void Increase(float value)
    {
      Assert.IsTrue(IsAlive, "Cannot increase health of a dead instance");
      CurrentHealth += value;
    }
  }
}