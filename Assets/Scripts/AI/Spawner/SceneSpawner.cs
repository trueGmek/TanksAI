using UnityEngine;
using Utils;
using Logger = Utils.Logger;

namespace AI.Core
{
  public class SceneSpawner : Spawner
  {
    [SerializeField] private Agent firstAgentPrefab;
    [SerializeField] private Agent secondAgentPrefab;

    //TODO: REWORK BOTH OF THE SPAWNPOINTS TO BE A HANDLE
    [SerializeField] private Transform spawnPointOne;
    [SerializeField] private Transform spawnPointTwo;

    private Agent _firstInstance;
    private Agent _secondInstance;

    public override void Spawn()
    {
      Logger.Log("Spawning agents", Tags.SPAWNER);
      _firstInstance = Instantiate(firstAgentPrefab, spawnPointOne);
      _secondInstance = Instantiate(secondAgentPrefab, spawnPointTwo);

      AgentManualTester.Bind(_firstInstance, _secondInstance);
    }
  }
}