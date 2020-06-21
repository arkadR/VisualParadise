using System.Collections.Generic;

namespace Assets.Scripts.Common.Extensions
{
  public static class IDictionaryExtensions
  {
    public static void AppendOrInsert<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
    {
      if (dictionary.ContainsKey(key))
        dictionary[key].Add(value);
      else
        dictionary[key] = new List<TValue> { value };
    }
  }
}
