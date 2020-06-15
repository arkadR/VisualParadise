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
      }

      var mesh = new Mesh();
      lineRenderer.BakeMesh(mesh, true);
      var meshCollider = line.GetComponent<MeshCollider>();
      meshCollider.sharedMesh = mesh;

      edge.gameObject = line;
    }

    (GameObject, LineRenderer) InstantiateLine(int positionCount)
    {
      var line = Instantiate(edgePrefab);
      var lineRenderer = line.GetComponent<LineRenderer>();
      lineRenderer.positionCount = positionCount;
      return (line, lineRenderer);
    }
  }
}
