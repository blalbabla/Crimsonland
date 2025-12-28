using Crimsonland.Data;
using Crimsonland.Utils;

namespace Crimsonland.Core;

public sealed class GameConfig
{
  public Dictionary<string, PerkDefinition> Perks;
  public Dictionary<string, WeaponDefinition> Weapons;
  public DifficultyDefinition Difficulty;
  public Dictionary<string, EnemyDefinition> Enemies;

  public static GameConfig Load()
  {
    return new GameConfig
    {
      Perks = Json.Load<Dictionary<string, PerkDefinition>>("Config/perks.json"),
      Weapons = Json.Load<Dictionary<string, WeaponDefinition>>("Config/weapons.json"),
      Difficulty = Json.Load<DifficultyDefinition>("Config/difficulty.json"),
      Enemies = Json.Load<Dictionary<string, EnemyDefinition>>("Config/enemies.json")
    };
  }
}
