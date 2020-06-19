using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Common.Extensions
{
  public static class IEnumerableExtensions
  {
    public static HashSet<T> ToSet<T>(this IEnumerable<T> enumerable) => new HashSet<T>(enumerable);

    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> enumerable, int count)
    {
      var list = enumerable.ToList();
      return list.Take(list.Count - count);
    }
  }
}
