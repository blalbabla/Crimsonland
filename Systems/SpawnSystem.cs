using Crimsonland.Core;

namespace Crimsonland.Systems;

public sealed class SpawnSystem
{
  private readonly GameConfig _config;
  private readonly EnemySystem _enemySystem;
  private float _spawnTimer;

  public SpawnSystem(GameConfig config, EnemySystem enemySystem)
  {
    _config = config;
    _enemySystem = enemySystem;
  }

  public void Update(Time time)
  {
    float di = _config.Difficulty.CalculateDI(time.TotalSeconds);
    float spawnRate = _config.Difficulty.CalculateSpawnRate(di);

    _spawnTimer += time.Delta;
    if (_spawnTimer >= 1f / spawnRate)
    {
      _enemySystem.Spawn(di);
      _spawnTimer = 0f;
    }
  }
}
