using System.Collections.Generic;

namespace Crimsonland.Components;

public sealed class PerkContainer
{
  private readonly Dictionary<string, int> _stacks = new();

  public IReadOnlyDictionary<string, int> Stacks => _stacks;

  public void Add(string perkId, int maxStacks)
  {
    if (!_stacks.ContainsKey(perkId))
      _stacks[perkId] = 0;

    if (maxStacks < 0 || _stacks[perkId] < maxStacks)
      _stacks[perkId]++;
  }

  public int GetStacks(string perkId)
  {
    return _stacks.TryGetValue(perkId, out int v) ? v : 0;
  }
}
