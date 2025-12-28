using System.Numerics;
using Raylib_cs;
using Crimsonland.Core;
using Crimsonland.Components;
using Crimsonland.Entities;
using Crimsonland.Data;

namespace Crimsonland.Systems;

public sealed class PlayerSystem
{
  // Публичные свойства для доступа из Game.cs и других систем
  public Player Player { get; private set; }
  public PlayerStats Stats { get; private set; }
  public Health Health; // health - структура, но поле класса может меняться, если мы не переприсваиваем struct целиком. 
                        // Лучше сделать Health class или оборачивать в ref методы, 
                        // но для POC допустим прямой доступ или свойство.
                        // В GDD Health был struct. Чтобы менять его поля, нужно аккуратно обращаться.
                        // Для удобства в System сделаем его полем.

  private readonly GameConfig _config;

  public PlayerSystem(GameConfig config)
  {
    _config = config;

    // Инициализация
    Player = new Player
    {
      Position = new Vector2(1280 / 2, 720 / 2) // Центр экрана
    };

    Stats = new PlayerStats();
    // Загружаем базовые статы из конфига, если они там есть, или дефолтные из класса
    Stats.Reset();

    Health = new Health
    {
      Max = Stats.MaxHP,
      Current = Stats.MaxHP
    };
  }

  public void Update(Time time)
  {
    // 1. Получаем ввод
    Vector2 input = InputSystem.GetMovementAxis();

    // 2. Двигаем игрока
    // Stats.MoveSpeed - это скорость в пикселях в секунду
    Player.Position += input * Stats.MoveSpeed * time.Delta;

    // 3. Ограничиваем экраном (простая коллизия со стенами)
    ClampPositionToScreen();

    // 4. Поворот (смотрим на мышь)
    Vector2 mousePos = InputSystem.GetMousePosition();
    Vector2 dir = mousePos - Player.Position;
    Player.Rotation = MathF.Atan2(dir.Y, dir.X) * (180 / MathF.PI);

    // Синхронизация MaxHP (если перки увеличили макс здоровье)
    if (Health.Max != Stats.MaxHP)
    {
      // Если макс здоровье выросло, увеличиваем и текущее (опционально)
      int diff = Stats.MaxHP - Health.Max;
      Health.Max = Stats.MaxHP;
      if (diff > 0) Health.Current += diff;
    }
  }

  public void Draw()
  {
    // Простая отрисовка для POC (Круг + линия направления)
    Raylib.DrawCircleV(Player.Position, 20, Color.Green);

    // Линия направления взгляда
    Vector2 dir = InputSystem.GetMousePosition() - Player.Position;
    if (dir.Length() > 0) dir = Vector2.Normalize(dir);
    Raylib.DrawLineV(Player.Position, Player.Position + dir * 30, Color.Yellow);
  }

  private void ClampPositionToScreen()
  {
    // Хардкод размеров экрана 1280x720, как в Game.cs
    // В идеале брать из Raylib.GetScreenWidth()
    float radius = 20f;
    Player.Position.X = Math.Clamp(Player.Position.X, radius, 1280 - radius);
    Player.Position.Y = Math.Clamp(Player.Position.Y, radius, 720 - radius);
  }
}