using System.Numerics;

namespace Crimsonland.Entities;

public sealed class Enemy
{
  public Vector2 Position;
  public Vector2 Velocity;

  public int HP;
  public int Damage;
  public float Speed;

  public float AttackCooldownTimer;
  public bool IsRanged;

  public float AttackRange;
}
