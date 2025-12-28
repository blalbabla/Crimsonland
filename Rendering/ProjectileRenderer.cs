using System.Collections.Generic;
using Raylib_cs;
using Crimsonland.Entities;

namespace Crimsonland.Rendering;

public static class ProjectileRenderer
{
  public static void Draw(IEnumerable<Projectile> projectiles)
  {
    foreach (var p in projectiles)
    {
      if (!p.Active) continue;
      Raylib.DrawCircleV(p.Position, p.Radius, Color.Yellow);
    }
  }
}