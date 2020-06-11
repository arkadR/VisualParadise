using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class EdgeGameObjectFactory : MonoBehaviour
  {
    public readonly int linePoints = 15;
    public GameObject edgePrefab;

    public GameObject CreateEdgeGameObject(Node fromNode, Node toNode)
    {
      var (line, lineRenderer) = InstantiateLine(2);
      lineRenderer.SetPosition(0, fromNode.Position);
      lineRenderer.SetPosition(1, toNode.Position);
      return line;
    }

    /// <summary>
    ///   Creates a curved edge using Beziér curve in *linePoints* steps.
    ///   Always curved upwards.
    /// </summary>
    public GameObject CreateCurvedEdgeGameObject(Node fromNode, Node toNode)
    {
      var startingPoint = fromNode.Position;
      var middlePoint = ((toNode.Position + fromNode.Position) / 2) + (Vector3.up * 5);
      var endingPoint = toNode.Position;

      //Could also be written as
      // t = step, p0 = start, p1 = middle, p2 = end
      // (1-t) ((1 - t) * p0 + t * p1) + t((1-t) * p1 + t * p2)
      // which is linear interpolation between points given by interpolating (p0 and p1), (p1 and p2)
      Vector3 Bezier(float step)
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
        var currentLineEnd = Bezier(i / (float)linePoints);
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
  }
}
