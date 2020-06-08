using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Node
  {
    public int id;
    public Point point;
    public VPoint vpoint;
    public APoint apoint;

    [NonSerialized] public UnityEngine.GameObject gameObject;

    public static Node EmptyNode(int id, UnityEngine.GameObject gameObject) => new Node
    {
      id = id,
      point = new Point(),
      apoint = new APoint(),
      vpoint = new VPoint(),
      gameObject = gameObject
    };

    public Vector3 Position
    {
      get => point.position;
      set
      {
        point.position = value;
        gameObject.transform.position = value;
      }
    }

    public Vector3 Rotation
    {
      get => point.rotation;
      set
      {
        point.rotation = value;
        gameObject.transform.rotation = Quaternion.Euler(value);
      }
    }

    public Vector3 Velocity
    {
      get => vpoint.velocity;
      set => vpoint.velocity = value;
    }

    public Vector3 AngularVelocity
    {
      get => vpoint.angularVelocity;
      set => vpoint.angularVelocity = value;
    }

    public Vector3 Acceleration
    {
      get => apoint.acceleration;
      set => apoint.acceleration = value;
    }

    public Vector3 AngularAcceleration
    {
      get => apoint.angularAcceleration;
      set => apoint.angularAcceleration = value;
    }
  }
}
