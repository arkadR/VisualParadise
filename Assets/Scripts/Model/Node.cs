using System;
using System.Runtime.Serialization;
using Assets.Scripts.Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Node : IEquatable<Node>
  {
    public int id;
    public string label;
    public int? classId;
    public Point point;
    public VPoint vpoint;
    public APoint apoint;

    [NonSerialized] public GameObject gameObject;
    [NonSerialized] public NodeClass nodeClass;

    public static Node EmptyNode(int id, GameObject gameObject)
    {
      var node = new Node
      {
        id = id,
        label = id.ToString(),
        point = new Point(),
        apoint = new APoint(),
        vpoint = new VPoint(),
        gameObject = gameObject
      };
      node.Text.text = node.label;
      return node;
    }

    public string DefaultLabel => id.ToString();

    public Text Text => gameObject.GetComponentInChildren<Text>();

    public void UpdateTextPosition(Camera camera) => Text.SetPositionOnScreen(camera.WorldToScreenPoint(Position));

    public void UpdateLabel(string label)
    {
      this.label = label;
      Text.text = label;
    }

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

    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
      if (string.IsNullOrEmpty(label))
        label = DefaultLabel;
    }
  }
}
