using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class APoint
  {
    public Vector3 Acceleration { get; set; }
    public Vector3 AngularAcceleration { get; set; }
  }
}
