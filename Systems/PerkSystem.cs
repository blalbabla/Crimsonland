using Crimsonland.Components;
using Crimsonland.Core;
using Crimsonland.Data;

namespace Crimsonland.Systems;

public sealed class PerkSystem
{
  private readonly GameConfig _config;

  public PerkSystem(GameConfig config)
  {
    _config = config;
  }

  public void ApplyPerks(PlayerStats stats, PerkContainer perks)
  {
    stats.Reset();

    // 1. additive
    foreach (var kv in perks.Stacks)
    {
      PerkDefinition def = _config.Perks[kv.Key];
      int stacks = kv.Value;

      if (def.Mode == "additive")
        ApplyAdditive(stats, def, stacks);
    }

    // 2. multiplier
    foreach (var kv in perks.Stacks)
    {
      PerkDefinition def = _config.Perks[kv.Key];
      int stacks = kv.Value;

      if (def.Mode == "multiplier")
        ApplyMultiplier(stats, def, stacks);
    }
  }

  private void ApplyAdditive(PlayerStats s, PerkDefinition d, int n)
  {
    if (d.Stat == "maxHp")
      s.MaxHP += (int)(d.Value * n);
  }

  private void ApplyMultiplier(PlayerStats s, PerkDefinition d, int n)
  {
    float factor = MathF.Pow(d.Value, n);

    if (d.Stat == "damage")
      s.Damage *= factor;
    else if (d.Stat == "fireRate")
      s.FireRate *= factor;
    else if (d.Stat == "moveSpeed")
      s.MoveSpeed *= factor;
  }
}
