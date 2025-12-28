namespace Crimsonland.Components;

public sealed class PlayerStats
{
  // base
  public float BaseDamage = 10;
  public float BaseFireRate = 4;
  public float BaseMoveSpeed = 200;
  public int BaseMaxHP = 100;

  // runtime (after perks)
  public float Damage;
  public float FireRate;
  public float MoveSpeed;
  public int MaxHP;

  public void Reset()
  {
    Damage = BaseDamage;
    FireRate = BaseFireRate;
    MoveSpeed = BaseMoveSpeed;
    MaxHP = BaseMaxHP;
  }
}
