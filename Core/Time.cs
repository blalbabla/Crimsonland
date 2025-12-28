using Raylib_cs;

namespace Crimsonland.Core;

public sealed class Time
{
  public float Delta { get; private set; }
  public float TotalSeconds { get; private set; }

  public void Update()
  {
    Delta = Raylib.GetFrameTime();
    TotalSeconds += Delta;
  }
}