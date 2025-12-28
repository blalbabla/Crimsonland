using System;
using System.Collections.Generic;
using System.Numerics;
using Crimsonland.Core;
using Crimsonland.Data;
using Crimsonland.Entities;
using Raylib_cs; // Нужно для проверки столкновений снарядов, если логика там

namespace Crimsonland.Systems;

public sealed class EnemySystem
{
  private readonly GameConfig _config;
  private readonly List<Enemy> _enemies = new();

  // Ссылка на ScoreSystem нужна для начисления очков
  private ScoreSystem _scoreSystem;

  public IReadOnlyList<Enemy> Enemies => _enemies;

  public EnemySystem(GameConfig config)
  {
    _config = config;
  }

  // Метод для внедрения зависимости (чтобы не менять конструктор GameConfig)
  public void SetScoreSystem(ScoreSystem scoreSystem)
  {
    _scoreSystem = scoreSystem;
  }

  public void Spawn(string enemyType, Vector2 position, float di)
  {
    // ... (код Spawn без изменений) ...
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

  //Примечание: Я добавил проверку столкновений Projectile-Enemy прямо в EnemySystem.Update.
  //В более сложной системе это было бы в CollisionSystem, но для завершения POC
  //это самый быстрый способ сделать игру играбельной.

  public void Update(Time time, Vector2 playerPosition, ProjectileSystem projectileSystem)
  {
    // 1. Проверка попаданий снарядов (интегрируем сюда для простоты POC)
    // В идеале это может быть в отдельной CollisionSystem
    foreach (var p in projectileSystem.ActiveProjectiles())
    {
      // Проходимся по врагам обратным циклом не нужно, т.к. снаряд удаляется
      // Но здесь мы просто помечаем.
      if (!p.Active) continue;

      foreach (var enemy in _enemies)
      {
        if (enemy.HP <= 0) continue; // Уже мертв

        if (Utils.Collision.CircleCircle(p.Position, p.Radius, enemy.Position, 10)) // 10 - радиус врага (хардкод для POC)
        {
          enemy.HP -= p.Damage;
          p.Active = false; // Снаряд уничтожен
          break; // Снаряд попал в одного врага
        }
      }
    }

    // 2. Обновление AI и удаление мертвых
    for (int i = _enemies.Count - 1; i >= 0; i--)
    {
      var enemy = _enemies[i];

      if (enemy.HP <= 0)
      {
        // Враг умер
        _scoreSystem?.AddScore(100); // +100 очков за убийство (можно вынести в конфиг)
        _enemies.RemoveAt(i);
        continue;
      }

      enemy.AttackCooldownTimer -= time.Delta;

      if (enemy.IsRanged)
        UpdateRanged(enemy, playerPosition, time.Delta);
      else
        UpdateMelee(enemy, playerPosition, time.Delta);
    }
  }

  // ... (Методы UpdateMelee, UpdateRanged без изменений) ...
  // ... (Методы DealDamageToPlayer, FireProjectile - заглушки остаются пока такими) ...

  private void UpdateMelee(Enemy enemy, Vector2 playerPos, float dt)
  {
    Vector2 dir = Vector2.Normalize(playerPos - enemy.Position);
    enemy.Position += dir * enemy.Speed * dt;

    float distance = Vector2.Distance(enemy.Position, playerPos);

    if (distance <= enemy.AttackRange && enemy.AttackCooldownTimer <= 0f)
    {
      // Тут мы должны нанести урон игроку. 
      // В архитектуре POC это делается через прямой вызов, 
      // но у нас нет ссылки на игрока в этом методе кроме позиции.
      // Для завершения цикла реализуем это в Game.Update через проверку коллизий 
      // или добавим событие. 
      // Для простоты оставим изменение таймера.
      enemy.AttackCooldownTimer = 0.8f;
    }
  }
  private void UpdateRanged(Enemy enemy, Vector2 playerPos, float dt)
  {
    Vector2 toPlayer = playerPos - enemy.Position;
    float distance = toPlayer.Length();
    Vector2 dir = Vector2.Normalize(toPlayer);

    if (distance > enemy.AttackRange)
    {
      enemy.Position += dir * enemy.Speed * dt;
    }
    else
    {
      enemy.Position -= dir * enemy.Speed * 0.5f * dt;
      if (enemy.AttackCooldownTimer <= 0f)
      {
        // FireProjectile(enemy.Position, dir, enemy.Damage);
        enemy.AttackCooldownTimer = 1.5f;
      }
    }
  }
}