using System;
using System.Collections.Generic;
using System.Numerics;
using Crimsonland.Core;
using Crimsonland.Entities; // Нужно для Player
using Crimsonland.Data;     // Нужно для DifficultyDefinition

namespace Crimsonland.Systems;

public sealed class SpawnSystem
{
  private readonly GameConfig _config;
  private readonly EnemySystem _enemySystem;
  private float _spawnTimer;
  private readonly Random _rng = new Random();

  public SpawnSystem(GameConfig config, EnemySystem enemySystem)
  {
    _config = config;
    _enemySystem = enemySystem;
  }

  // Изменили сигнатуру: теперь принимаем Player, чтобы взять его Level и Position
  public void Update(Time time, Player player)
  {
    // ИСПРАВЛЕНИЕ 1: Передаем player.Level
    float di = _config.Difficulty.CalculateDI(time.TotalSeconds, player.Level);

    float spawnRate = _config.Difficulty.CalculateSpawnRate(di);

    _spawnTimer += time.Delta;
    if (_spawnTimer >= 1f / spawnRate)
    {
      SpawnEnemy(di, player.Position);
      _spawnTimer = 0f;
    }
  }

  private void SpawnEnemy(float di, Vector2 playerPos)
  {
    // 1. Выбираем тип врага
    // Собираем список всех типов, которые разблокированы для текущего DI
    var validTypes = new List<string>();
    foreach (var type in _config.Enemies.Keys)
    {
      if (_config.Difficulty.IsEnemyTypeUnlocked(type, di))
      {
        validTypes.Add(type);
      }
    }

    // Если ничего не разблокировано (баг конфига), берем "basic" или ничего
    if (validTypes.Count == 0) return;

    // Случайный тип из доступных
    string selectedType = validTypes[_rng.Next(validTypes.Count)];

    // 2. Выбираем позицию (по кругу вокруг игрока, за пределами экрана)
    // Экран примерно 1280x720, берем радиус побольше
    float angle = (float)(_rng.NextDouble() * Math.PI * 2);
    float distance = 900f; // Достаточно далеко, чтобы заспавниться за экраном

    Vector2 offset = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * distance;
    Vector2 spawnPos = playerPos + offset;

    // ИСПРАВЛЕНИЕ 2: Вызываем Spawn с тремя аргументами
    _enemySystem.Spawn(selectedType, spawnPos, di);
  }
}