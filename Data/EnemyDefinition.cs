using System.Numerics;

namespace Crimsonland.Data;

public sealed class EnemyDefinition
{
  public string Id { get; set; }

  public int BaseHP { get; set; }
  public int BaseDamage { get; set; }
  public float BaseSpeed { get; set; }

  public float AttackRange { get; set; }
  public float AttackCooldown { get; set; }

  public bool IsRanged { get; set; }
}
