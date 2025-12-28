using Crimsonland.Core;
using Crimsonland.Systems;
using Raylib_cs;

namespace Crimsonland;

public sealed class Game : IDisposable
{
  private readonly PlayerSystem playerSystem;
  private readonly EnemySystem enemySystem;
  private readonly SpawnSystem spawnSystem;
  private readonly ProjectileSystem projectileSystem;

  private readonly GameConfig config;

  public Game()
  {
    Raylib.InitWindow(1280, 720, "Crimsonland Clone");
    Raylib.SetTargetFPS(60);

    config = GameConfig.Load("Config");

    playerSystem = new PlayerSystem(config);
    enemySystem = new EnemySystem(config);
    spawnSystem = new SpawnSystem(config, enemySystem);
    projectileSystem = new ProjectileSystem();
  }

  public void Run()
  {
    while (!Raylib.WindowShouldClose())
    {
      float dt = Raylib.GetFrameTime();

      Update(dt);
      Draw();
    }
  }

  private void Update(float dt)
  {
    playerSystem.Update(dt);
    spawnSystem.Update(dt, playerSystem.Player);
    enemySystem.Update(dt, playerSystem.Player);
    projectileSystem.Update(dt);
  }

  private void Draw()
  {
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    playerSystem.Draw();
    enemySystem.Draw();
    projectileSystem.Draw();

    Raylib.EndDrawing();
  }

  public void Dispose()
  {
    Raylib.CloseWindow();
  }
}
