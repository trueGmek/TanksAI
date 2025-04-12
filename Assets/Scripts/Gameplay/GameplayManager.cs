using System;
using UnityEngine;

namespace Gameplay
{
  public class GameplayManager : MonoBehaviour
  {
    [SerializeField] private AI.Core.Spawner spawner;

    private void Start()
    {
      spawner.Spawn();
    }
  }
}