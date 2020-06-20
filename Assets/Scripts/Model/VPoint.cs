using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class VPoint
  {
    public Vector3 Velocity { get; set; }
    public Vector3 AngularVelocity { get; set; }
  }
}
