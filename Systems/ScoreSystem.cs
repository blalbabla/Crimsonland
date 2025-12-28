using System;
using System.IO;
using System.Text.Json;
using Crimsonland.Data;
using Crimsonland.Core;

namespace Crimsonland.Systems;

public sealed class ScoreSystem
{
  private const string SaveFileName = "savegame.json";

  public int CurrentScore { get; private set; }
  public float SurvivalTime { get; private set; }

  public int HighScore { get; private set; }
  public float BestTime { get; private set; }

  public ScoreSystem()
  {
    Load();
  }

  public void Update(Time time)
  {
    SurvivalTime += time.Delta;
  }

  public void AddScore(int amount)
  {
    CurrentScore += amount;
  }

  public void CheckRecords()
  {
    bool changed = false;
    if (CurrentScore > HighScore)
    {
      HighScore = CurrentScore;
      changed = true;
    }

    if (SurvivalTime > BestTime)
    {
      BestTime = SurvivalTime;
      changed = true;
    }

    if (changed)
    {
      Save();
    }
  }

  public void Reset()
  {
    CurrentScore = 0;
    SurvivalTime = 0f;
  }

  private void Save()
  {
    var data = new SaveData
    {
      HighScore = HighScore,
      LongestSurvivalTime = BestTime
    };

    try
    {
      string json = JsonSerializer.Serialize(data);
      File.WriteAllText(SaveFileName, json);
    }
    catch
    {
      // Игнорируем ошибки ввода-вывода в POC
    }
  }

  private void Load()
  {
    if (!File.Exists(SaveFileName)) return;

    try
    {
      string json = File.ReadAllText(SaveFileName);
      var data = JsonSerializer.Deserialize<SaveData>(json);
      if (data != null)
      {
        HighScore = data.HighScore;
        BestTime = data.LongestSurvivalTime;
      }
    }
    catch
    {
      // Файл поврежден или старый формат
    }
  }
}