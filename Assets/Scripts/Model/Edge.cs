using System;
using Assets.Scripts.Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Edge : IEquatable<Edge>
  {
    public int id;
    public string label;
    public int from;
    public int to;
    public int weight;

    [NonSerialized] public GameObject gameObject;
    [NonSerialized] public Node nodeFrom;
    [NonSerialized] public Node nodeTo;

    public Text Text => gameObject.GetComponentInChildren<Text>();

    public string DefaultLabel
    {
      get => $"{from}-{to}";
    }

    public void UpdateTextPosition()
    {
      var (x1, y1, z1) = Camera.main.WorldToScreenPoint(nodeFrom.Position);
      var (x2, y2, z2) = Camera.main.WorldToScreenPoint(nodeTo.Position);
      var tan = (y1 - y2) / (x1 - x2);
      tan = float.IsNaN(tan) ? 0 : tan;
      var angle = Mathf.Atan(tan) * Mathf.Rad2Deg;
      var centerOfEdgeOnScreen = new Vector3(x1 + x2, y1 + y2, z1 + z2) / 2;
      Text.SetPositionOnScreen(centerOfEdgeOnScreen);
      Text.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

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
