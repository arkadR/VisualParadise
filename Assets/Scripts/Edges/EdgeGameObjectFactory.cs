using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public class EdgeGameObjectFactory : MonoBehaviour
  {
    private const float _additionalColliderLength = 0.1f;
    private const float _edgeColliderRadius = 0.09f;
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
      var colliderGameObject = new GameObject(Constants.ColliderGameObjectName);
      colliderGameObject.transform.parent = line.transform;
      var capsule = colliderGameObject.AddComponent<CapsuleCollider>();
      SetColliderProperties(colliderGameObject, firstEdgePoint, secondEdgePoint);
    }

    public void SetColliderProperties(GameObject colliderGameObject, Vector3 firstEdgePoint, Vector3 secondEdgePoint)
    {
      var capsule = colliderGameObject.GetComponent<CapsuleCollider>();
      capsule.radius = _edgeColliderRadius;
      capsule.height = Vector3.Distance(secondEdgePoint, firstEdgePoint) + _additionalColliderLength;
      //capsule collider direction can be 0, 1 or 2 corresponding to the X, Y and Z axes, respectively
      capsule.direction = 2;

      var collider = capsule.gameObject;
      var direction = secondEdgePoint - firstEdgePoint;
      collider.transform.position = (firstEdgePoint + secondEdgePoint) * 0.5f;
      collider.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
  }
}
