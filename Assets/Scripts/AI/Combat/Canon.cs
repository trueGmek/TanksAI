using UnityEngine;

namespace AI.Combat
{
  public abstract class Canon : MonoBehaviour
  {
    public abstract void Shoot(Vector3 direction);
  }
}