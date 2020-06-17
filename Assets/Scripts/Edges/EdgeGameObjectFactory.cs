using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public class EdgeGameObjectFactory : MonoBehaviour
  {
    public GameObject edgePrefab;
    public LineFactory lineFactory;

    public void CreateGameObjectEdgesFor(IList<Edge> edges, bool labelVisibility)
    {
      var startingPoint = edges.First().nodeFrom.Position;
      var endingPoint = edges.First().nodeTo.Position;
      var edgePointsForEachEdge = lineFactory.GetLinePositionsFor(startingPoint, endingPoint, edges.Count);
      for (var i = 0; i < edges.Count; i++)
      {
        var edge = edges[i];
        if (edge.gameObject != null)
          Destroy(edge.gameObject);
        CreateNewEdgeGameObjectFor(edge, edgePointsForEachEdge[i]);
        edge.Text.text = edge.label;
        edge.Text.enabled = labelVisibility;
      }
    }

    void CreateNewEdgeGameObjectFor(Edge edge, IList<Vector3> edgePoints)
    {
      var edgePointsCount = edgePoints.Count;
      var (line, lineRenderer) = InstantiateLine(edgePointsCount);
      for (var i = 0; i < edgePointsCount; i++)
      {
        lineRenderer.SetPosition(i, edgePoints[i]);

        if (i > 0) GenerateCollider(line, edgePoints[i - 1], edgePoints[i]);
      }

      edge.gameObject = line;
    }

    (GameObject, LineRenderer) InstantiateLine(int positionCount)
    {
      var line = Instantiate(edgePrefab);
      var lineRenderer = line.GetComponent<LineRenderer>();
      lineRenderer.positionCount = positionCount;
      return (line, lineRenderer);
    }

    private void GenerateCollider(GameObject line, Vector3 firstEdgePoint, Vector3 secondEdgePoint)
    {
      var collider = new GameObject("Collider");
      collider.transform.parent = line.transform;
      var capsule = collider.AddComponent<CapsuleCollider>();
      collider.transform.parent = capsule.transform;

      capsule.radius = 0.09f;
      capsule.height = Vector3.Distance(secondEdgePoint, firstEdgePoint);
      capsule.direction = 2;

      collider.transform.position = (firstEdgePoint + secondEdgePoint) * 0.5f;

      var direction = secondEdgePoint - firstEdgePoint;
      collider.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
  }
}
