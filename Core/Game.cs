using System;
using Crimsonland.Core;
using Crimsonland.Systems;
using Crimsonland.UI;
using Crimsonland.Components;
using Crimsonland.Rendering; // Не забудьте этот using
using Raylib_cs;

namespace Crimsonland;

public enum GameState
{
  Playing,
  GameOver
}

public sealed class Game : IDisposable
{
  // 1. Здесь только ОБЪЯВЛЯЕМ поля
  private readonly GameConfig config;
  private readonly Time time;

  private readonly PlayerSystem playerSystem;
  private readonly EnemySystem enemySystem;
  private readonly SpawnSystem spawnSystem;
  private readonly ProjectileSystem projectileSystem;
  private readonly WeaponSystem weaponSystem;
  private readonly ScoreSystem scoreSystem;

  // Рендерер объявляем, но НЕ создаем здесь
  private readonly Renderer renderer;

  private GameState state = GameState.Playing;

  public Game()
  {
    Raylib.InitWindow(1280, 720, "Crimsonland Clone");
    Raylib.SetTargetFPS(60);

    config = GameConfig.Load();
    time = new Time();

    // 2. Инициализируем системы ПО ПОРЯДКУ
    projectileSystem = new ProjectileSystem();
    enemySystem = new EnemySystem(config); // Создаем врагов
    spawnSystem = new SpawnSystem(config, enemySystem);

    playerSystem = new PlayerSystem(config); // Создаем игрока
    weaponSystem = new WeaponSystem(config, projectileSystem);
    scoreSystem = new ScoreSystem();

    enemySystem.SetScoreSystem(scoreSystem);

    // 3. Создаем Renderer ТОЛЬКО СЕЙЧАС
    // К этому моменту playerSystem, enemySystem и projectileSystem уже существуют (не null)
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
        {
          Restart();
        }
      }

      Draw();
    }
  }

  private void Update()
  {
    scoreSystem.Update(time);
    playerSystem.Update(time);
    weaponSystem.Update(time, playerSystem.Player.Position);
    projectileSystem.Update(time.Delta);
    spawnSystem.Update(time, playerSystem.Player);
    enemySystem.Update(time, playerSystem.Player.Position, projectileSystem);
    CheckCollisionsPlayerEnemies();
  }

  private void CheckCollisionsPlayerEnemies()
  {
    foreach (var enemy in enemySystem.Enemies)
    {
      // Простая коллизия: если расстояние меньше суммы радиусов
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
    // Примечание: для полного рестарта нужно также очистить списки врагов и снарядов,
    // но для POC достаточно сбросить стейт.
    state = GameState.Playing;
  }

  private void Draw()
  {
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Raylib_cs.Color.Black);

    if (state == GameState.Playing)
    {
      // Здесь renderer уже создан и не null
      renderer.Draw();

      // Временный HUD через Hud.cs
      // Hud.DrawText(scoreSystem, playerSystem.Health); // Если вы реализовали это
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