using System.Runtime.CompilerServices;
using UnityEngine;

namespace DefaultNamespace
{
  public static class Vector3Extension
  {
    public static void Deconstruct(this Vector3 vector3, out float x, out float y, out float z)
    {
      x = vector3.x;
      y = vector3.y;
      z = vector3.z;
    }
  }
}