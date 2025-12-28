using System.Collections.Generic;
using System.Numerics;
using Crimsonland.Entities;

namespace Crimsonland.Systems;

public sealed class ProjectileSystem
{
  private const int PoolSize = 2048;
  private readonly Projectile[] _pool;

  public ProjectileSystem()
  {
    _pool = new Projectile[PoolSize];
    for (int i = 0; i < PoolSize; i++)
      _pool[i] = new Projectile();
  }

  public void Spawn(Vector2 position, Vector2 velocity, int damage, float radius)
  {
    foreach (var p in _pool)
    {
      if (!p.Active)
      {
        p.Active = true;
        p.Position = position;
        p.Velocity = velocity;
        p.Damage = damage;
        p.Radius = radius;
        return;
      }
    }
    // пул исчерпан — снаряд теряется (как в оригинале)
  }

  public void Update(float dt)
  {
    foreach (var p in _pool)
    {
      if (!p.Active) continue;

      p.Position += p.Velocity * dt;

      if (IsOutOfBounds(p.Position))
        p.Active = false;
    }
  }

  public IEnumerable<Projectile> ActiveProjectiles()
  {
    foreach (var p in _pool)
      if (p.Active)
        yield return p;
  }

  private bool IsOutOfBounds(Vector2 pos)
  {
    return pos.X < -50 || pos.X > 1330 || pos.Y < -50 || pos.Y > 770;
  }
}
