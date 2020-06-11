using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Edge : IEquatable<Edge>
  {
    public int from;

    [NonSerialized] public GameObject gameObject;
    public int id;

    [NonSerialized] public Node nodeFrom;
    [NonSerialized] public Node nodeTo;
    public int to;
    public int weight;

    public bool Equals(Edge other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return id == other.id;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      if (obj.GetType() != GetType())
        return false;
      return Equals((Edge)obj);
    }

    public override int GetHashCode() => id;

    public static bool operator ==(Edge left, Edge right) => Equals(left, right);

    public static bool operator !=(Edge left, Edge right) => !Equals(left, right);
  }
}
