using System;
using System.Collections.Generic;

namespace Crimsonland.Data;

public sealed class DifficultyDefinition
{
  // --- DI weights ---
  public float TimeWeight { get; set; }
  public float LevelWeight { get; set; }

  // --- Spawn ---
  public float BaseSpawnRate { get; set; }
  public float SpawnRateMultiplier { get; set; }
  public float MaxSpawnRate { get; set; }

  // --- Enemy scaling ---
  public float EnemyHpMultiplier { get; set; }
  public float EnemyDamageMultiplier { get; set; }
  public float EnemySpeedMultiplier { get; set; }

  // --- Enemy unlock thresholds ---
  public Dictionary<string, float> EnemyTypeThresholds { get; set; }

  // =========================
  // DI CALCULATION
  // =========================

  public float CalculateDI(float timeSeconds, int playerLevel)
  {
    return timeSeconds * TimeWeight
         + playerLevel * LevelWeight;
  }

  // =========================
  // SPAWN RATE
  // =========================

  public float CalculateSpawnRate(float di)
  {
    float rate = BaseSpawnRate + di * SpawnRateMultiplier;
    return MathF.Min(rate, MaxSpawnRate);
  }

  // =========================
  // ENEMY SCALING
  // =========================

  public int ScaleEnemyHP(int baseHp, float di)
  {
    return (int)MathF.Round(
        baseHp * (1f + di * EnemyHpMultiplier)
    );
  }

  public int ScaleEnemyDamage(int baseDamage, float di)
  {
    return (int)MathF.Round(
        baseDamage * (1f + di * EnemyDamageMultiplier)
    );
  }

  public float ScaleEnemySpeed(float baseSpeed, float di)
  {
    return baseSpeed * (1f + di * EnemySpeedMultiplier);
  }

  // =========================
  // ENEMY TYPE AVAILABILITY
  // =========================

  public bool IsEnemyTypeUnlocked(string enemyType, float di)
  {
    if (!EnemyTypeThresholds.TryGetValue(enemyType, out float threshold))
      return false;

    return di >= threshold;
  }
}
