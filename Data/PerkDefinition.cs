namespace Crimsonland.Data;

public sealed class PerkDefinition
{
  public string Id { get; set; }

  public string Stat { get; set; }
  // damage, fireRate, moveSpeed, maxHp, armor, etc.

  public string Mode { get; set; }
  // additive | multiplier | tick

  public float Value { get; set; }

  public int MaxStacks { get; set; }  // -1 = infinite
}
