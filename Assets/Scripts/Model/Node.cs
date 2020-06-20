using System;
using System.Runtime.Serialization;
using Assets.Scripts.Common.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Node : IEquatable<Node>
  {
    [JsonProperty] public int Id { get; private set; }
    [JsonProperty] public string Label { get; private set; }
    [JsonProperty] public int? ClassId { get; set; }
    [JsonProperty] public Point Point { get; private set; }
    [JsonProperty] public VPoint VPoint { get; private set; }
    [JsonProperty] public APoint APoint { get; private set; }

    [NonSerialized] public GameObject gameObject;
    [NonSerialized] public NodeClass nodeClass;

    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
      if (string.IsNullOrEmpty(Label))
        Label = DefaultLabel;
    }

    public static Node EmptyNode(int id, GameObject gameObject)
    {
      var node = new Node
      {
        Id = id,
        Label = id.ToString(),
        Point = new Point(),
        APoint = new APoint(),
        VPoint = new VPoint(),
        gameObject = gameObject
      };
      node.Text.text = node.Label;
      return node;
    }

    public string DefaultLabel => Id.ToString();

    public Text Text => gameObject.GetComponentInChildren<Text>();

    public void UpdateTextPosition(Camera camera) => Text.SetPositionOnScreen(camera.WorldToScreenPoint(Position));

    public void UpdateLabel(string label)
    {
      this.Label = label;
      Text.text = label;
    }

    public Vector3 Position
    {
      get => Point.Position;
      set
      {
        Point.Position = value;
        gameObject.transform.position = value;
      }
    }

    public Vector3 Rotation
    {
      get => Point.Rotation;
      set
      {
        Point.Rotation = value;
        gameObject.transform.rotation = Quaternion.Euler(value);
      }
    }

    public Vector3 Velocity
    {
      get => VPoint.Velocity;
      set => VPoint.Velocity = value;
    }

    public Vector3 AngularVelocity
    {
      get => VPoint.AngularVelocity;
      set => VPoint.AngularVelocity = value;
    }

    public Vector3 Acceleration
    {
      get => APoint.Acceleration;
      set => APoint.Acceleration = value;
    }

    public Vector3 AngularAcceleration
    {
      get => APoint.AngularAcceleration;
      set => APoint.AngularAcceleration = value;
    }

    public bool Equals(Node other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((Node)obj);
    }

    public override int GetHashCode() => Id;

    public static bool operator ==(Node left, Node right) => Equals(left, right);

    public static bool operator !=(Node left, Node right) => !Equals(left, right);

  }
}
