using System.Numerics;

namespace Crimsonland.Entities;

public sealed class Projectile
{
  public Vector2 Position;
  public Vector2 Velocity;

  public int Damage;
  public float Radius;

  public bool Active;
}
