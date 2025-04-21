using Combat;
using Utils;

namespace AI.Combat
{
  public class TankDamageProcessor : DamageProcessor
  {
    public override void ApplyDamage(DamageData damageData)
    {
      Logger.Log($"Tank: {gameObject.name} received damage!", Tags.AGENT);
    }
  }
}