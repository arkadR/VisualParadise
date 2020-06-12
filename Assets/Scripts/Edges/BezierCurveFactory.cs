using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public class BezierCurveFactory : MonoBehaviour
  {
    public int LinePoints { get; } = 15;

    public List<Vector3> BezierCurve(Vector3 startingPoint, Vector3 middlePoint, Vector3 endingPoint)
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

      var curvePoints = new List<Vector3>();
      for (var i = 0; i < LinePoints; i++)
      {
        curvePoints.Add(
          CurvePoint(i / (float)(LinePoints - 1)));
      }

      return curvePoints;
    }
  }
}
