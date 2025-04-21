using AI.Core;
using UnityEngine;
using Utils;
using Logger = Utils.Logger;

namespace AI.Spawner
{
  public class SceneSpawner : Spawner
  {
    [SerializeField] private Agent firstAgentPrefab;
    [SerializeField] private Agent secondAgentPrefab;

    [SerializeField] private Transform spawnPointOne;
    [SerializeField] private Transform spawnPointTwo;


    public override void Spawn()
    {
      Logger.Log("Spawning agents", Tags.SPAWNER);
      
      Agent firstInstance = Instantiate(firstAgentPrefab, spawnPointOne.position, spawnPointOne.rotation);
      Agent secondInstance = Instantiate(secondAgentPrefab, spawnPointTwo.position, spawnPointTwo.rotation);

      AgentManualTester.Bind(firstInstance, secondInstance);
    }
  }
}