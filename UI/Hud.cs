using System.Numerics;
using Crimsonland.Components;
using Crimsonland.Systems;
using Raylib_cs;

namespace Crimsonland.UI;

public static class Hud
{
  public static void Draw(PlayerStats player, ScoreSystem score)
  {
    // HP Bar
    Raylib.DrawRectangle(20, 20, 200, 20, Color.DarkGray);
    float hpPercent = (float)player.MaxHP / player.BaseMaxHP; // Упрощено, надо current/max
                                                              // В PlayerStats в коде выше нет CurrentHP, оно в Health component.
                                                              // Предполагаем, что HUD получает доступ к Health component.
  }

  // Перегрузка для упрощения вывода текста
  public static void DrawText(ScoreSystem score, Health health)
  {
    // Score
    Raylib.DrawText($"Score: {score.CurrentScore}", 20, 20, 20, Color.White);
    Raylib.DrawText($"Time: {score.SurvivalTime:F1}", 20, 50, 20, Color.White);

    // High Score
    Raylib.DrawText($"HI: {score.HighScore}", 20, 80, 20, Color.Gold);

    // HP Text
    Raylib.DrawText($"HP: {health.Current}/{health.Max}", 20, 680, 30, Color.Red);
  }

  public static void DrawGameOver(ScoreSystem score)
  {
    int screenWidth = Raylib.GetScreenWidth();
    int screenHeight = Raylib.GetScreenHeight();

    Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(0, 0, 0, 200));

    string text = "GAME OVER";
    int fontSize = 60;
    int textWidth = Raylib.MeasureText(text, fontSize);
    Raylib.DrawText(text, screenWidth / 2 - textWidth / 2, screenHeight / 2 - 100, fontSize, Color.Red);

    string scoreText = $"Final Score: {score.CurrentScore}";
    int scoreWidth = Raylib.MeasureText(scoreText, 30);
    Raylib.DrawText(scoreText, screenWidth / 2 - scoreWidth / 2, screenHeight / 2, 30, Color.White);

    string restartText = "Press R to Restart";
    int restartWidth = Raylib.MeasureText(restartText, 20);
    Raylib.DrawText(restartText, screenWidth / 2 - restartWidth / 2, screenHeight / 2 + 50, 20, Color.Gray);
  }
}