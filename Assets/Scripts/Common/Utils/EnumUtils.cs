using System;

namespace Assets.Scripts.Common.Utils
{
  //https://stackoverflow.com/questions/6413804/why-does-casting-int-to-invalid-enum-value-not-throw-exception
  /// <summary>
  ///   Utility methods for enum values. This static type will fail to initialize
  ///   (throwing a <see cref="ArgumentException" />) if
  ///   you try to provide a value that is not an enum.
  /// </summary>
  /// <typeparam name="T">An enum type. </typeparam>
  public static class EnumUtils<T> where T : struct, IConvertible // Try to get as much of a static check as we can.
  {
    // The .NET framework doesn't provide a compile-checked
    // way to ensure that a type is an enum, so we have to check when the type
    // is statically invoked.
    static EnumUtils()
    {
      // Throw Exception on static initialization if the given type isn't an enum.
      if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
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
