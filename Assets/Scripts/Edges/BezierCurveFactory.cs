using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public class BezierCurveFactory 
  {
    public List<Vector3> BezierCurve3(Vector3 startingPoint, Vector3 middlePoint, Vector3 endingPoint, int linePoints)
    {
      var steps = Enumerable.Range(0, linePoints).Select(i => i / (float)(linePoints - 1));
      var curvePoints = steps.Select(step => CurvePoint(step, startingPoint, middlePoint, endingPoint));

      return curvePoints.ToList();
    }

    public List<Vector3> BezierCurve4(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int linePoints)
    {
      var steps = Enumerable.Range(0, linePoints).Select(i => i / (float)(linePoints - 1));
      var curvePoints = steps.Select(step =>
                          (1 - step) * CurvePoint(step, p1, p2, p3) +
                        step * CurvePoint(step, p2, p3, p4));

      return curvePoints.ToList();
    }

    //Could also be written as
    // t = step, p0 = start, p1 = middle, p2 = end
    // (1-t) ((1 - t) * p0 + t * p1) + t((1-t) * p1 + t * p2)
    // which is linear interpolation between points given by interpolating (p0 and p1), (p1 and p2)
    Vector3 CurvePoint(float step, Vector3 startingPoint, Vector3 middlePoint, Vector3 endingPoint)
    {
      step = Mathf.Clamp01(step);
      var oneMinusStep = 1 - step;
      return (oneMinusStep * oneMinusStep * startingPoint)
             + (2f * oneMinusStep * step * middlePoint)
             + (step * step * endingPoint);
    }

  }
}
