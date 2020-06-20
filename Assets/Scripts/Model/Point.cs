using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Point
  {
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
  }
}
