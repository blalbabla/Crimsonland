using System.Collections.Generic;
using Raylib_cs;
using Crimsonland.Entities;

namespace Crimsonland.Rendering;

public static class EnemyRenderer
{
  public static void Draw(IEnumerable<Enemy> enemies)
  {
    foreach (var enemy in enemies)
    {
      Color color = Color.Red;

      // Простая цветовая дифференциация по типу (если нужно)
      if (enemy.IsRanged) color = Color.Purple;
      else if (enemy.Speed > 100) color = Color.Orange; // Быстрые

      Raylib.DrawCircleV(enemy.Position, 15, color);

      // Отображение HP (опционально, для отладки)
      // Raylib.DrawText(enemy.HP.ToString(), (int)enemy.Position.X - 10, (int)enemy.Position.Y - 25, 10, Color.White);
    }
  }
}