using System.Numerics;

namespace Crimsonland.Entities;

public sealed class Player
{
  public Vector2 Position;
  public float Rotation; // Угол поворота (в градусах или радианах, для отрисовки)
  public int Level = 1;
  public float Experience;

  // Ссылки на текущее состояние, если мы хотим хранить их в Entity, 
  // но в нашей архитектуре Stats и Health лежат рядом в PlayerSystem 
  // или как компоненты. Для простоты POC оставим тут только позиционные данные.
}