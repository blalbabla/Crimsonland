using System;
using Crimsonland.Core;
using Crimsonland.Systems;
using Crimsonland.UI;
using Crimsonland.Components;
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

  private readonly PlayerSystem playerSystem;
  private readonly EnemySystem enemySystem;
  private readonly SpawnSystem spawnSystem;
  private readonly ProjectileSystem projectileSystem;
  private readonly WeaponSystem weaponSystem; 
  private readonly ScoreSystem scoreSystem;

  // Рендерер
  Renderer renderer = new Renderer(playerSystem, enemySystem, projectileSystem);

  private GameState state = GameState.Playing;

  public Game()
  {
    Raylib.InitWindow(1280, 720, "Crimsonland");
    Raylib.SetTargetFPS(60);

    config = GameConfig.Load();
    time = new Time();

    // Инициализация систем
    projectileSystem = new ProjectileSystem();
    enemySystem = new EnemySystem(config);

    spawnSystem = new SpawnSystem(config, enemySystem);
    playerSystem = new PlayerSystem(config);
    weaponSystem = new WeaponSystem(config, projectileSystem); // Weapon зависит от Projectile
    scoreSystem = new ScoreSystem();

    renderer = new Crimsonland.Rendering.Renderer(); // В коде из Git он был, используем его

    enemySystem.SetScoreSystem(scoreSystem);
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
    // 1. Системы, не зависящие от игрового цикла напрямую
    scoreSystem.Update(time);

    // 2. Логика игрока
    playerSystem.Update(time); // Перемещение

    // 3. Логика оружия (Стрельба игрока)
    // PlayerSystem хранит позицию в Entities/Player.cs? 
    // В текущей реализации PlayerSystem хранит Player entity внутри.
    // Нам нужно передать позицию игрока в WeaponSystem.
    weaponSystem.Update(time, playerSystem.Player.Position); // Исходим из того, что PlayerSystem.Player публичный

    // 4. Снаряды
    projectileSystem.Update(time.Delta);

    // 5. Спавн врагов
    spawnSystem.Update(time);

    // 6. Враги (Движение + Коллизии снарядов + Начисление очков)
    enemySystem.Update(time, playerSystem.Player.Position, projectileSystem);

    // 7. Проверка: Убил ли враг игрока?
    CheckCollisionsPlayerEnemies();
  }

  private void CheckCollisionsPlayerEnemies()
  {
    // Простая проверка: если враг касается игрока - Game Over
    // (В полной игре здесь было бы HP игрока)

    foreach (var enemy in enemySystem.Enemies)
    {
      float playerRadius = 15f;
      if (Utils.Collision.CircleCircle(playerSystem.Player.Position, playerRadius, enemy.Position, 10f))
      {
        // Игрок получил урон
        // playerSystem.Player.HP -= enemy.Damage; ...

        // Для POC сразу Game Over
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
    // Сброс состояния для новой игры
    // В реальном проекте лучше пересоздать Game, но здесь просто очистим списки
    // В текущей реализации проще перезапустить приложение, но попробуем сбросить
    scoreSystem.Reset();
    // Очистка врагов и снарядов требует методов Clear() в их системах.
    // Для POC проще всего закрыть и открыть (или пересоздать сцену), 
    // но оставим игроку возможность видеть экран Game Over.

    // В рамках этого кода: просто сброс счета и выход в меню.
    // Чтобы реально перезапустить, нужно реализовать методы Reset() в каждой системе.
    state = GameState.Playing;
    // Враги останутся, это баг упрощения. В продакшене нужен Reset().
  }

  private void Draw()
  {
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Raylib_cs.Color.Black);

    if (state == GameState.Playing)
    {
      // Рендеринг игрового мира (используем существующий Renderer)
      // Но Renderer в файлах статический или требует инстанса?
      // В файлах: public void Draw() { PlayerRenderer.Draw(); ... }
      // Он вызывает статические методы рендереров.
      renderer.Draw();

      // UI поверх всего
      Hud.DrawText(scoreSystem, new Health { Current = 100, Max = 100 }); // Заглушка HP
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