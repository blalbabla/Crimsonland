using System.Numerics;

namespace Crimsonland.Entities;

public enum PickupType
{
  Health,
  Weapon,
  Powerup
}

public sealed class Pickup
{
  public Vector2 Position;
  public PickupType Type;
  public string Value; // Например, ID оружия ("shotgun") или количество HP ("50")
  public bool Active = true;
  public float LifeTime = 10.0f; // Время жизни на земле
}