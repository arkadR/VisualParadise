using System.Collections.Generic;

namespace Assets.Scripts.Common.Extensions
{
  public static class EnumerableTExtension
  {
    public static HashSet<T> ToSet<T>(this IEnumerable<T> enumerable) => new HashSet<T>(enumerable);
  }
}
