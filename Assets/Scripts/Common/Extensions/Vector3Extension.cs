using UnityEngine;

namespace Assets.Scripts.Common.Extensions
{
  public static class Vector3Extension
  {
    public static void Deconstruct(this Vector3 vector3, out float x, out float y, out float z)
    {
      (x, y, z) = (vector3.x, vector3.y, vector3.z);
    }

    public static (string, string, string) ToStringTuple(this Vector3 vector3)
    {
      var (x, y, z) = (vector3.x, vector3.y, vector3.z);
      return (x.ToString(), y.ToString(), z.ToString());
    }
  }
}
