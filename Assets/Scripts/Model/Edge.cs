using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Edges;
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

    [NonSerialized] public GameObject labelGameObject;
    [NonSerialized] public Node nodeFrom;
    [NonSerialized] public Node nodeTo;
    [NonSerialized] public SegmentGroup segmentGroup;

    public Text Text => labelGameObject.GetComponent<Text>();

    public string DefaultLabel => $"{from}-{to}";

    public bool Equals(Edge other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return id == other.id;
    }

    public void UpdateTextPosition(Camera camera)
    {
      Text.SetPositionOnScreen(GetLabelPosition(camera));
      Text.rectTransform.rotation = GetLabelAngle(camera);
    }

    public void UpdateLabel(string label)
    {
      this.label = label;
      Text.text = label;
    }

    Vector3 GetLabelPosition(Camera camera)
    {
      var position = segmentGroup.MiddleSegment.transform.position;
      return camera.WorldToScreenPoint(position);
    }

    Vector3 CurvedLineMiddlePoint(Camera camera, LineRenderer lineRenderer)
    {
      var middleIndex = lineRenderer.positionCount / 2;
      var middlePointPosition = lineRenderer.GetPosition(middleIndex);
      return camera.WorldToScreenPoint(middlePointPosition);
    }

    Vector3 StraightLineMiddle(Camera camera)
    {
      var middle = (nodeFrom.Position + nodeTo.Position) / 2;
      return camera.WorldToScreenPoint(middle);
    }

    Quaternion GetLabelAngle(Camera camera)
    {
      var (x1, y1, z1) = camera.WorldToScreenPoint(nodeFrom.Position);
      var (x2, y2, z2) = camera.WorldToScreenPoint(nodeTo.Position);
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
