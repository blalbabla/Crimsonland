using System.Numerics;
using Raylib_cs;
using Crimsonland.Entities;
using Crimsonland.Systems; // Для InputSystem

namespace Crimsonland.Rendering;

public static class PlayerRenderer
{
  public static void Draw(Player player)
  {
    if (player == null) return;

    // Тело игрока
    Raylib.DrawCircleV(player.Position, 20, Color.Green);

    // Направление взгляда (прицел)
    // Используем InputSystem или просто Raylib для получения мыши
    Vector2 mousePos = Raylib.GetMousePosition();
    Vector2 dir = mousePos - player.Position;
    if (dir.Length() > 0)
    {
      dir = Vector2.Normalize(dir);
      Raylib.DrawLineV(player.Position, player.Position + dir * 30, Color.Yellow);
    }
  }
}