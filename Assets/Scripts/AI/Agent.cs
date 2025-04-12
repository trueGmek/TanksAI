using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;

        public void Start()
        {
            Debug.Log($"Hello my name is {gameObject.name}");
        }

       private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            Debug.Log(mousePos);
        }
    }
}
