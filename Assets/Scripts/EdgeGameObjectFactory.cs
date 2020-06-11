using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class EdgeGameObjectFactory : MonoBehaviour
  {
    readonly List<GameObject> debugSpheres = new List<GameObject>();
    public readonly int linePoints = 15;
    public bool debug;
    public GameObject debugSpherePrefab;
    public GameObject edgePrefab;

    public void CreateGameObjectEdgesFor(ISet<Edge> edges)
    {
      if (edges.Count == 0)
        return;
      if (edges.Count == 1)
      {
        CreateEdgeGameObjectForSingle(edges.Single());
        return;
      }

      CreateEdgeGameObjectsForMultiple(edges);
    }

    void CreateEdgeGameObjectForSingle(Edge edge)
    {
      var (line, lineRenderer) = InstantiateLine(2);
      lineRenderer.SetPosition(0, edge.nodeFrom.Position);
      lineRenderer.SetPosition(1, edge.nodeTo.Position);
      if (edge.gameObject != null)
        Destroy(edge.gameObject);
      edge.gameObject = line;
    }

    void CreateEdgeGameObjectsForMultiple(ISet<Edge> edges)
    {
      ClearDebugSpheres();
      var rotation = 360f / edges.Count;
      var currentRotation = 0f;
      var startingPoint = edges.First().nodeFrom.Position;
      var endingPoint = edges.First().nodeTo.Position;
      var middlePoint = (endingPoint + startingPoint) / 2;
      var rotationAxis = (endingPoint - startingPoint).normalized;
      var normalVector = Vector3.Cross(Vector3.up, rotationAxis).normalized;

      if (debug)
        AddDebugSphere(middlePoint);

      foreach (var edge in edges)
      {
        CreateCurvedEdgeGameObject(edge,
          currentRotation,
          startingPoint,
          middlePoint,
          endingPoint,
          rotationAxis,
          normalVector);

        currentRotation += rotation;
      }
    }

    /// <summary>
    ///   Creates and attaches to Edge a curved GO line using Beziér curve in *linePoints* steps rotated by
    ///   <paramref name="rotation" />
    /// </summary>
    void CreateCurvedEdgeGameObject(Edge edge,
      float rotation,
      Vector3 startingPoint,
      Vector3 middlePoint,
      Vector3 endingPoint,
      Vector3 rotationAxis,
      Vector3 normalVector)
    {
      var rotatedPoint = middlePoint + (Quaternion.AngleAxis(rotation, rotationAxis) * normalVector * 5);

      if (edge.gameObject != null)
        Destroy(edge.gameObject);
      edge.gameObject = BezierCurve(startingPoint, rotatedPoint, endingPoint);

      if (debug)
        AddDebugSphere(rotatedPoint);
    }

    GameObject BezierCurve(Vector3 startingPoint, Vector3 middlePoint, Vector3 endingPoint)
    {
      //Could also be written as
      // t = step, p0 = start, p1 = middle, p2 = end
      // (1-t) ((1 - t) * p0 + t * p1) + t((1-t) * p1 + t * p2)
      // which is linear interpolation between points given by interpolating (p0 and p1), (p1 and p2)
      Vector3 CurvePoint(float step)
      {
        step = Mathf.Clamp01(step);
        var oneMinusStep = 1 - step;
        return (oneMinusStep * oneMinusStep * startingPoint)
               + (2f * oneMinusStep * step * middlePoint)
               + (step * step * endingPoint);
      }

      var (line, lineRenderer) = InstantiateLine(linePoints + 2);

      lineRenderer.SetPosition(0, startingPoint);
      for (var i = 0; i < linePoints; i++)
      {
        var currentLineEnd = CurvePoint(i / (float)linePoints);
        lineRenderer.SetPosition(i + 1, currentLineEnd);
      }

      lineRenderer.SetPosition(linePoints + 1, endingPoint);
      return line;
    }

    (GameObject, LineRenderer) InstantiateLine(int positionCount)
    {
      var line = Instantiate(edgePrefab);
      var lineRenderer = line.GetComponent<LineRenderer>();
      lineRenderer.positionCount = positionCount;
      return (line, lineRenderer);
    }

    void ClearDebugSpheres()
    {
      foreach (var sphere in debugSpheres)
      {
        Destroy(sphere);
      }

      debugSpheres.Clear();
    }

    void AddDebugSphere(Vector3 position) =>
      debugSpheres.Add(Instantiate(debugSpherePrefab, position, Quaternion.identity));
  }
}
