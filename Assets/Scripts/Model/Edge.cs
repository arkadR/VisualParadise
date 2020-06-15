using System;
using Assets.Scripts.Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Edge : IEquatable<Edge>
  {
    public int from;

    [NonSerialized] public GameObject gameObject;
    public int id;
    public string label;
    [NonSerialized] public Node nodeFrom;
    [NonSerialized] public Node nodeTo;
    public int to;
    public int weight;

    public Text Text => gameObject.GetComponentInChildren<Text>();

    public string DefaultLabel => $"{from}-{to}";

    public bool Equals(Edge other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return id == other.id;
    }

    public void UpdateTextPosition()
    {
      Text.SetPositionOnScreen(LabelPosition());
      Text.rectTransform.rotation = LabelAngle();
    }

    public void UpdateLabel(string label)
    {
      this.label = label;
      Text.text = label;
    }

    Vector3 LabelPosition()
    {
      var lineRenderer = gameObject.GetComponent<LineRenderer>();
      return
        lineRenderer.positionCount == 2
          ? StraightLineMiddle()
          : CurvedLineMiddlePoint(lineRenderer);
    }

    Vector3 CurvedLineMiddlePoint(LineRenderer lineRenderer)
    {
      var middleIndex = lineRenderer.positionCount / 2;
      var middlePointPosition = lineRenderer.GetPosition(middleIndex);
      return Camera.main.WorldToScreenPoint(middlePointPosition);
    }

    Vector3 StraightLineMiddle()
    {
      var middle = (nodeFrom.Position + nodeTo.Position) / 2;
      return Camera.main.WorldToScreenPoint(middle);
    }

    Quaternion LabelAngle()
    {
      var (x1, y1, z1) = Camera.main.WorldToScreenPoint(nodeFrom.Position);
      var (x2, y2, z2) = Camera.main.WorldToScreenPoint(nodeTo.Position);
      var tan = (y1 - y2) / (x1 - x2);
      tan = float.IsNaN(tan) ? 0 : tan;
      var angle = Mathf.Atan(tan) * Mathf.Rad2Deg;
      return Quaternion.Euler(0, 0, angle);
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
