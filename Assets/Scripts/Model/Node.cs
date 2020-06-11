using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Node : IEquatable<Node>
  {
    public APoint apoint;

    [NonSerialized] public GameObject gameObject;
    public int id;
    public Point point;
    public VPoint vpoint;

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

    public bool Equals(Node other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return id == other.id;
    }

    public static Node EmptyNode(int id, GameObject gameObject) => new Node
    {
      id = id,
      point = new Point(),
      apoint = new APoint(),
      vpoint = new VPoint(),
      gameObject = gameObject
    };

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((Node)obj);
    }

    public override int GetHashCode() => id;

    public static bool operator ==(Node left, Node right) => Equals(left, right);

    public static bool operator !=(Node left, Node right) => !Equals(left, right);
  }
}
