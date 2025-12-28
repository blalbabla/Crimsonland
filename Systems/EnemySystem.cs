using System;
using System.Collections.Generic;
using System.Numerics;
using Crimsonland.Core;
using Crimsonland.Data;
using Crimsonland.Entities;

namespace Crimsonland.Systems;

public sealed class EnemySystem
{
  private readonly GameConfig _config;
  private readonly List<Enemy> _enemies = new();

  public IReadOnlyList<Enemy> Enemies => _enemies;

  public EnemySystem(GameConfig config)
  {
    _config = config;
  }

  public void Spawn(string enemyType, Vector2 position, float di)
  {
    EnemyDefinition def = _config.Enemies[enemyType];

    var enemy = new Enemy
    {
      Position = position,
      Speed = _config.Difficulty.ScaleEnemySpeed(def.BaseSpeed, di),
      HP = _config.Difficulty.ScaleEnemyHP(def.BaseHP, di),
      Damage = _config.Difficulty.ScaleEnemyDamage(def.BaseDamage, di),
      IsRanged = def.IsRanged,
      AttackRange = def.AttackRange,
      AttackCooldownTimer = 0f
    };

    _enemies.Add(enemy);
  }

  public void Update(Time time, Vector2 playerPosition)
  {
    foreach (var enemy in _enemies)
    {
      enemy.AttackCooldownTimer -= time.Delta;

      if (enemy.IsRanged)
        UpdateRanged(enemy, playerPosition, time.Delta);
      else
        UpdateMelee(enemy, playerPosition, time.Delta);
    }
  }

  // =========================
  // MELEE AI
  // =========================

  private void UpdateMelee(Enemy enemy, Vector2 playerPos, float dt)
  {
    Vector2 dir = Vector2.Normalize(playerPos - enemy.Position);
    enemy.Position += dir * enemy.Speed * dt;

    float distance = Vector2.Distance(enemy.Position, playerPos);

    if (distance <= enemy.AttackRange && enemy.AttackCooldownTimer <= 0f)
    {
      DealDamageToPlayer(enemy.Damage);
      enemy.AttackCooldownTimer = 0.8f;
    }
  }

  // =========================
  // RANGED AI
  // =========================

  private void UpdateRanged(Enemy enemy, Vector2 playerPos, float dt)
  {
    Vector2 toPlayer = playerPos - enemy.Position;
    float distance = toPlayer.Length();

    Vector2 dir = Vector2.Normalize(toPlayer);

    if (distance > enemy.AttackRange)
    {
      // approach
      enemy.Position += dir * enemy.Speed * dt;
    }
    else
    {
      // держит дистанцию (минимальное отступление)
      enemy.Position -= dir * enemy.Speed * 0.5f * dt;

      if (enemy.AttackCooldownTimer <= 0f)
      {
        FireProjectile(enemy.Position, dir, enemy.Damage);
        enemy.AttackCooldownTimer = 1.5f;
      }
    }
  }

  // =========================
  // PLACEHOLDERS
  // =========================

  private void DealDamageToPlayer(int damage)
  {
    // реализуется в PlayerSystem
  }

  private void FireProjectile(Vector2 origin, Vector2 direction, int damage)
  {
    // реализуется в ProjectileSystem
  }
}
