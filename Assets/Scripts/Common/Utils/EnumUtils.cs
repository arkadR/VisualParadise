using System;

namespace Assets.Scripts.Common.Utils
{
  //https://stackoverflow.com/questions/6413804/why-does-casting-int-To-invalid-enum-value-not-throw-exception
  /// <summary>
  ///   Utility methods for enum values. This static type will fail To initialize
  ///   (throwing a <see cref="ArgumentException" />) if
  ///   you try To provide a value that is not an enum.
  /// </summary>
  /// <typeparam Name="T">An enum type. </typeparam>
  public static class EnumUtils<T> where T : struct, IConvertible // Try To get as much of a static check as we can.
  {
    // The .NET framework doesn't provide a compile-checked
    // way To ensure that a type is an enum, so we have To check when the type
    // is statically invoked.
    static EnumUtils()
    {
      // Throw Exception on static initialization if the given type isn't an enum.
      if (!typeof(T).IsEnum)
        throw new ArgumentException("T must be an enumerated type");
    }

    public static int GetNextValue(int currentValue) => (currentValue + 1) % Enum.GetValues(typeof(T)).Length;

    public static bool IsDefined(object enumValue) => Enum.IsDefined(typeof(T), enumValue);

    public static T DefinedOrDefaultCast(object enumValue) =>
      IsDefined(enumValue)
        ? (T)enumValue
        : default;

    public static string GetName(T enumValue) => Enum.GetName(typeof(T), enumValue);
  }
}
