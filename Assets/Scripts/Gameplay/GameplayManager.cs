using System;
using AI.Spawner;
using UnityEngine;

namespace Gameplay
{
  public class GameplayManager : MonoBehaviour
  {
    [SerializeField] private Spawner spawner;

    private void Start()
    {
      spawner.Spawn();
    }
  }
}