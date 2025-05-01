using AI.Core;
using AI.Lua;
using Lua;
using UnityEngine;
using UnityEngine.Assertions;
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

    //TODO: THIS IS ONLY TEMPORARY
    [SerializeField] private LuaManager luaManager;


    public override void Spawn()
    {
      Logger.Log("Spawning agents", Tags.SPAWNER);

      Agent firstInstance = Instantiate(firstAgentPrefab, spawnPointOne.position, spawnPointOne.rotation);
      Agent secondInstance = Instantiate(secondAgentPrefab, spawnPointTwo.position, spawnPointTwo.rotation);

      AgentManualTester.Bind(firstInstance, secondInstance);

      
      Assert.IsNotNull(luaManager, "luaManager != null");
      var aiBinder = new AiBindings(secondInstance);
      aiBinder.Bind(luaManager.State);
    }
  }
}