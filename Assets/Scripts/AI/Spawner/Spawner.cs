using AI.Tests;
using UnityEngine;

namespace AI.Core
{
  public abstract class Spawner : MonoBehaviour
  {
    [SerializeField] private AgentManualTester agentManualTester;

    protected AgentManualTester AgentManualTester => agentManualTester;

    public abstract void Spawn();
  }
}