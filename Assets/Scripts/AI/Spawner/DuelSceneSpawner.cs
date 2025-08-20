using AI.Core;
using AI.Lua;
using UnityEngine;
using Utils;
using Logger = Utils.Logger;

namespace AI.Spawner
{
  public class DuelSceneSpawner : Spawner
  {
    [SerializeField] private LuaAgentRunner firstAgentPrefab;
    [SerializeField] private LuaAgentRunner secondAgentPrefab;

    [SerializeField] private Transform spawnPointOne;
    [SerializeField] private Transform spawnPointTwo;

    public override void Spawn()
    {
      Logger.Log("Spawning agents", Tags.SPAWNER);

      LuaAgentRunner firstInstance = Instantiate(firstAgentPrefab, spawnPointOne.position, spawnPointOne.rotation);
      LuaAgentRunner secondInstance = Instantiate(secondAgentPrefab, spawnPointTwo.position, spawnPointTwo.rotation);

      firstInstance.SetOpponent(secondInstance.GetComponent<Agent>());
      secondInstance.SetOpponent(firstInstance.GetComponent<Agent>());

      firstInstance.Initialize();
      secondInstance.Initialize();

      firstInstance.StartAgent();
      secondInstance.StartAgent();
    }
  }
}
