using System.Numerics;
using Raylib_cs;

namespace Crimsonland.Systems;

public static class InputSystem
{
  public static Vector2 GetMovementAxis()
  {
    Vector2 input = Vector2.Zero;

    if (Raylib.IsKeyDown(KeyboardKey.W)) input.Y -= 1;
    if (Raylib.IsKeyDown(KeyboardKey.S)) input.Y += 1;
    if (Raylib.IsKeyDown(KeyboardKey.A)) input.X -= 1;
    if (Raylib.IsKeyDown(KeyboardKey.D)) input.X += 1;

    // Нормализация, чтобы движение по диагонали не было быстрее
    if (input.LengthSquared() > 0)
    {
      input = Vector2.Normalize(input);
    }

    return input;
  }

  public static Vector2 GetMousePosition()
  {
    return Raylib.GetMousePosition();
  }
}