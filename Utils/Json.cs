using System.IO;
using System.Text.Json;

namespace Crimsonland.Utils;

public static class Json
{
  private static readonly JsonSerializerOptions _options = new()
  {
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    AllowTrailingCommas = true,
    IncludeFields = true
  };

  public static T Load<T>(string path)
  {
    // Проверка пути, чтобы не падать с непонятной ошибкой
    if (!File.Exists(path))
    {
      // Пытаемся найти относительно выходной директории (bin/Debug/netX.X)
      string localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
      if (File.Exists(localPath))
      {
        path = localPath;
      }
      else
      {
        throw new FileNotFoundException($"Config file not found: {path}");
      }
    }

    string json = File.ReadAllText(path);
    var result = JsonSerializer.Deserialize<T>(json, _options);

    if (result == null)
      throw new InvalidOperationException($"Failed to deserialize config: {path}");

    return result;
  }
}