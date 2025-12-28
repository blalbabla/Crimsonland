using System;
using Crimsonland.Core;
using Crimsonland.Systems;
using Crimsonland.UI; // Если используется HUD
using Crimsonland.Rendering;
using Raylib_cs;

namespace Crimsonland;

public enum GameState
{
  Playing,
  GameOver
}

public sealed class Game : IDisposable
{
  private readonly GameConfig config;
  private readonly Time time;

  // Системы
  private readonly PlayerSystem playerSystem;
  private readonly EnemySystem enemySystem;
  private readonly SpawnSystem spawnSystem;
  private readonly ProjectileSystem projectileSystem;
  private readonly WeaponSystem weaponSystem; // <--- Добавили систему оружия
  private readonly ScoreSystem scoreSystem;

  private readonly Renderer renderer;

  private GameState state = GameState.Playing;

  public Game()
  {
    Raylib.InitWindow(1280, 720, "Crimsonland Clone");
    Raylib.SetTargetFPS(60);

    config = GameConfig.Load();
    time = new Time();

    // Порядок инициализации важен!
    projectileSystem = new ProjectileSystem(); // 1. Снаряды
    enemySystem = new EnemySystem(config);
    spawnSystem = new SpawnSystem(config, enemySystem);
    playerSystem = new PlayerSystem(config);

    // 2. Оружие (зависит от конфига и системы снарядов)
    weaponSystem = new WeaponSystem(config, projectileSystem);

    scoreSystem = new ScoreSystem();
    enemySystem.SetScoreSystem(scoreSystem);

    // 3. Рендерер получает ссылки на все системы, чтобы их рисовать
    renderer = new Renderer(playerSystem, enemySystem, projectileSystem);
  }

  public void Run()
  {
    while (!Raylib.WindowShouldClose())
    {
      time.Update();

      if (state == GameState.Playing)
      {
        Update();
      }
      else if (state == GameState.GameOver)
      {
        if (Raylib.IsKeyPressed(KeyboardKey.R))
          Restart();
      }

      Draw();
    }
  }

  private void Update()
  {
    scoreSystem.Update(time);
    playerSystem.Update(time);

    // <--- ГЛАВНОЕ: Обновляем логику оружия (проверяет нажатие ЛКМ)
    weaponSystem.Update(time, playerSystem.Player.Position);

    // Обновляем полет пуль
    projectileSystem.Update(time.Delta);

    spawnSystem.Update(time, playerSystem.Player);

    // Проверяем попадания пуль во врагов
    enemySystem.Update(time, playerSystem.Player.Position, projectileSystem);

    CheckCollisionsPlayerEnemies();
  }

  private void CheckCollisionsPlayerEnemies()
  {
    foreach (var enemy in enemySystem.Enemies)
    {
      // Если враг коснулся игрока
      if (Utils.Collision.CircleCircle(playerSystem.Player.Position, 15f, enemy.Position, 10f))
      {
        GameOver();
        break;
      }
    }
  }

  private void GameOver()
  {
    state = GameState.GameOver;
    scoreSystem.CheckRecords();
  }

  private void Restart()
  {
    scoreSystem.Reset();
    // Для полного рестарта тут нужно очистить врагов и пули, 
    // но пока просто вернемся в игру
    state = GameState.Playing;
  }

  private void Draw()
  {
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    if (state == GameState.Playing)
    {
      renderer.Draw();
      // Hud.DrawText(scoreSystem, playerSystem.Health); 
    }
    else
    {
      Hud.DrawGameOver(scoreSystem);
    }

    Raylib.EndDrawing();
  }

  public void Dispose()
  {
    Raylib.CloseWindow();
  }
}