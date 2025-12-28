using System.Numerics;

namespace Crimsonland.Utils;

public static class Collision
{
  public static bool CircleCircle(
      Vector2 a, float ra,
      Vector2 b, float rb)
  {
    return Vector2.DistanceSquared(a, b) <= (ra + rb) * (ra + rb);
  }
}
