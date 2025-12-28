namespace Crimsonland.Data;

public sealed class WeaponDefinition
{
  public string Id { get; set; }

  public int Damage { get; set; }
  public float RateOfFire { get; set; }   // shots per second
  public float Spread { get; set; }       // degrees
  public int Ammo { get; set; }           // -1 = infinite
  public float ReloadTime { get; set; }

  public float ProjectileSpeed { get; set; }
  public float ProjectileRadius { get; set; }

  public bool IsHitscan { get; set; }
}
