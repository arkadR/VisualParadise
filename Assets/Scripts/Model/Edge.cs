using System;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Edges;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Edge : IEquatable<Edge>
  {
    [JsonProperty] public int Id { get; private set; }
    [JsonProperty] public string Label { get; private set; }
    [JsonProperty] public int From { get; private set; }
    [JsonProperty] public int To { get; private set; }
    [JsonProperty] public int? EdgeClassId { get; private set; }
    [JsonProperty] public int? StartClassId { get; private set; }
    [JsonProperty] public int? EndClassId { get; private set; }

    public int weight;

    [NonSerialized] public GraphElementClass EdgeClass;
    [NonSerialized] public GraphElementClass StartClass;
    [NonSerialized] public GraphElementClass EndClass;
    [NonSerialized] public GameObject labelGameObject;
    [NonSerialized] public Node nodeFrom;
    [NonSerialized] public Node nodeTo;
    [NonSerialized] public SegmentGroup segmentGroup;

    public Edge(int id, string label, int from, int to, int? classId)
    {
      Id = id;
      From = from;
      To = to;
      Label = string.IsNullOrEmpty(label) ? DefaultLabel : label;
      EdgeClassId = classId;
    }

    public static Edge BetweenNodes(int id, string label, Node node1, Node node2)
    {
      var edge = new Edge(id, label, node1.Id, node2.Id, null)
      {
        nodeFrom = node1, 
        nodeTo = node2
      };
      return edge;
    }

    [JsonIgnore]
    public Text Text => labelGameObject.GetComponentInChildren<Text>();

    public string DefaultLabel => $"{From}-{To}";

    public void SetLabel(string label)
    {
      Label = label;
      Text.text = label;
    }

    public void UpdateTextPosition(Camera camera)
    {
      Text.SetPositionOnScreen(GetLabelPosition(camera));
      Text.rectTransform.rotation = GetLabelAngle(camera);
    }
    
    Vector3 GetLabelPosition(Camera camera)
    {
      var position = segmentGroup.MiddleSegment.transform.position;
      return camera.WorldToScreenPoint(position);
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

    public bool Equals(Edge other)
    {
      if (ReferenceEquals(null, other))
        return false;
      if (ReferenceEquals(this, other))
        return true;
      return Id == other.Id;
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

    public override int GetHashCode() => Id;

    public static bool operator ==(Edge left, Edge right) => Equals(left, right);

    public static bool operator !=(Edge left, Edge right) => !Equals(left, right);
  }
}
