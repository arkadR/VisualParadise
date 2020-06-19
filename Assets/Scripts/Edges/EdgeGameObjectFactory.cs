using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Edges
{
  public class EdgeGameObjectFactory : MonoBehaviour
  {
    const float c_edgeThickness = 0.1f;
    public GameObject edgePrefab;
    public LineFactory lineFactory;

    public List<(SegmentGroup, GameObject)> CreateGameObjectEdgesFor(IList<Edge> edges, bool labelVisibility)
    {
      var startingPoint = edges.First().nodeFrom.Position;
      var endingPoint = edges.First().nodeTo.Position;
      var edgePointsForEachEdge = lineFactory.GetLinePositionsFor(startingPoint, endingPoint, edges.Count);
      var segmentGroups = new List<(SegmentGroup, GameObject)>();
      for (var i = 0; i < edges.Count; i++)
      {
        var edge = edges[i];
        var edgePoints = edgePointsForEachEdge[i];
        var segmentGameObjects = Enumerable
          .Range(0, edgePoints.Count - 1)
          .Select(_ => CreateCylinder())
          .ToArray();

        var segments = new SegmentGroup(segmentGameObjects);
        segments.PlaceAlongPoints(edgePoints);
        var text = new GameObject("Text");
        var t = text.AddComponent<Text>();
        t.text = edge.label;
        t.enabled = labelVisibility;
        segmentGroups.Add((segments, text));
      }

      return segmentGroups;
    }

    private GameObject CreateCylinder()
    {
      var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
      cylinder.transform.localScale = new Vector3(c_edgeThickness, 1, c_edgeThickness);
      return cylinder;
    }
  }
}
