using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Edges
{
  public class EdgeGameObjectFactory : MonoBehaviour
  {
    const float c_edgeThicknessMultiplier = 0.1f;
    public GameObject labelPrefab;
    public LineFactory lineFactory;
    public MaterialCache _materialCache;
    public void Start()
    {
      _materialCache = FindObjectOfType<MaterialCache>();
    }

    public List<(SegmentGroup, GameObject)> CreateGameObjectEdgesFor(IList<Edge> edges, bool labelVisibility)
    {
      if (_materialCache == null)
        _materialCache = FindObjectOfType<MaterialCache>();

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
          .Select(_ => CreateCylinder(edge.Class?.Thickness ?? 1f))
          .ToArray();

        var material = _materialCache.GetByTexturePath(edge.Class?.TexturePath);
        foreach (var segmentGameObject in segmentGameObjects)
        {
          segmentGameObject.GetComponent<Renderer>().material = material;
        }

        var segments = new SegmentGroup(segmentGameObjects);
        segments.PlaceAlongPoints(edgePoints);
        var text = Instantiate(labelPrefab);
        text.GetComponentInChildren<Text>().enabled = labelVisibility;
        text.GetComponentInChildren<Text>().text = edge.Label;
        segmentGroups.Add((segments, text));
      }

      return segmentGroups;
    }

    private GameObject CreateCylinder(float thickness)
    {
      var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
      cylinder.transform.localScale = new Vector3(
        c_edgeThicknessMultiplier * thickness, 
        1, 
        c_edgeThicknessMultiplier * thickness);
      return cylinder;
    }
  }
}
