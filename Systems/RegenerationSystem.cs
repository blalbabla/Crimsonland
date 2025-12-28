using Crimsonland.Components;
using Crimsonland.Core;

namespace Crimsonland.Systems;

public sealed class RegenerationSystem
{
  private float _timer;

  public void Update(float dt, Health health, PerkContainer perks, GameConfig config)
  {
    _timer += dt;

    if (_timer < 2f)
      return;

    _timer = 0f;

    foreach (var kv in perks.Stacks)
    {
      var def = config.Perks[kv.Key];
      if (def.Mode == "tick" && def.Stat == "hpRegen")
      {
        health.Current = Math.Min(
            health.Current + (int)(def.Value * kv.Value),
            health.Max
        );
      }
    }
  }
}
