using AI.Core;
using AI.Lua;
using AI.Tests;
using Lua;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using Logger = Utils.Logger;

namespace AI.Spawner
{
  public class SceneSpawner : Spawner
  {
    [SerializeField] private AgentManualTester agentManualTester;

    [SerializeField] private Agent firstAgentPrefab;
    [SerializeField] private Agent secondAgentPrefab;

    [SerializeField] private Transform spawnPointOne;
    [SerializeField] private Transform spawnPointTwo;

    [SerializeField] private StandaloneLuaRunner luaRunner;

    public override void Spawn()
    {
      Logger.Log("Spawning agents", Tags.SPAWNER);

      Agent firstInstance = Instantiate(firstAgentPrefab, spawnPointOne.position, spawnPointOne.rotation);
      Agent secondInstance = Instantiate(secondAgentPrefab, spawnPointTwo.position, spawnPointTwo.rotation);

      agentManualTester.Bind(firstInstance, secondInstance);

      Assert.IsNotNull(luaRunner, "luaRunner != null");
      var aiBinder = new AgentBinder(secondInstance);
      aiBinder.Bind(luaRunner.Runner.State);

      var opponentBinder = new OpponentBinder(firstInstance);
      opponentBinder.Bind(luaRunner.Runner.State);
    }
  }
}
