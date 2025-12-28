using System;
using System.Numerics;
using Crimsonland.Data;
using Crimsonland.Core;
using Crimsonland.Systems;
using Raylib_cs;

namespace Crimsonland.Systems;

public sealed class WeaponSystem
{
  private readonly WeaponDefinition _weapon;
  private readonly ProjectileSystem _projectiles;

  private float _fireCooldown;

  public WeaponSystem(GameConfig config, ProjectileSystem projectiles)
  {
    _weapon = config.Weapons["pistol"];
    _projectiles = projectiles;
  }

  public void Update(Time time, Vector2 origin)
  {
    _fireCooldown -= time.Delta;

    if (Raylib.IsMouseButtonDown(MouseButton.Left))
    {
      if (_fireCooldown <= 0f)
      {
        Fire(origin);
        _fireCooldown = 1f / _weapon.RateOfFire;
      }
    }
  }

  private void Fire(Vector2 origin)
  {
    Vector2 mouse = Raylib.GetMousePosition();
    Vector2 dir = Vector2.Normalize(mouse - origin);

    dir = ApplySpread(dir, _weapon.Spread);

    if (_weapon.IsHitscan)
    {
      FireHitscan(origin, dir);
    }
    else
    {
      _projectiles.Spawn(
          origin,
          dir * _weapon.ProjectileSpeed,
          _weapon.Damage,
          _weapon.ProjectileRadius
      );
    }
  }

  private void FireHitscan(Vector2 origin, Vector2 dir)
  {
    // проверка столкновений сразу
    // реализуется через EnemySystem
  }

  private Vector2 ApplySpread(Vector2 dir, float degrees)
  {
    float angle = MathF.Atan2(dir.Y, dir.X);
    float spread = Raylib.GetRandomValue(-(int)degrees, (int)degrees);
    angle += MathF.PI / 180f * spread;
    return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
  }
}
