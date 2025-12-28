namespace Crimsonland.Rendering;

public sealed class Renderer
{
  public void Draw()
  {
    PlayerRenderer.Draw();
    EnemyRenderer.Draw();
    ProjectileRenderer.Draw();
  }
}
