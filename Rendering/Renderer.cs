using Crimsonland.Systems;

namespace Crimsonland.Rendering;

public sealed class Renderer
{
  private readonly PlayerSystem _playerSystem;
  private readonly EnemySystem _enemySystem;
  private readonly ProjectileSystem _projectileSystem;

  // Внедряем зависимости через конструктор
  public Renderer(PlayerSystem playerSystem, EnemySystem enemySystem, ProjectileSystem projectileSystem)
  {
    _playerSystem = playerSystem;
    _enemySystem = enemySystem;
    _projectileSystem = projectileSystem;
  }

  public void Draw()
  {
    PlayerRenderer.Draw(_playerSystem.Player);
    EnemyRenderer.Draw(_enemySystem.Enemies);
    ProjectileRenderer.Draw(_projectileSystem.ActiveProjectiles());
  }
}