using System.Collections.Generic;
using Assets.Scripts.Common.Utils;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public class LineFactory : MonoBehaviour
  {
    //the further the point is (bigger value) the stronger will the curve be
    const int CurvePointDistance = 5;
    public BezierCurveFactory bezierCurveFactory;
    public bool debug;
    public DebugSphereFactory debugSphereFactory;

    /// <summary>
    ///   Returns a list of points that make up a curved, rotated edge when <paramref name="numberOfLines" /> > 1
    ///   and a straight, two points line when it is == 1
    /// </summary>
    public IList<IList<Vector3>> GetLinePositionsFor(Vector3 startingPoint, Vector3 endingPoint, int numberOfLines)
    {
      switch (numberOfLines)
      {
        case 0:
          return new List<IList<Vector3>>();
        case 1:
          return GetEdgePointsForSingleEdge(startingPoint, endingPoint);
        default:
          return GetEdgePointsForMultipleEdges(startingPoint, endingPoint, numberOfLines);
      }
    }

    IList<IList<Vector3>> GetEdgePointsForSingleEdge(Vector3 startingPoint, Vector3 endingPoint) =>
      new List<IList<Vector3>> {new List<Vector3> {startingPoint, endingPoint}};

    IList<IList<Vector3>> GetEdgePointsForMultipleEdges(Vector3 startingPoint, Vector3 endingPoint, int numberOfLines)
    {
      var rotation = 360f / numberOfLines;
      var currentRotation = 0f;
      var middlePoint = (endingPoint + startingPoint) / 2;
      var rotationAxis = (endingPoint - startingPoint).normalized;
      var normalVector = Vector3.Cross(Vector3.up, rotationAxis).normalized;

      if (debug)
      {
        debugSphereFactory.ClearDebugSpheres();
        debugSphereFactory.AddDebugSphere(middlePoint);
      }

      var curvePointsForEdges = new List<IList<Vector3>>();

      for (var i = 0; i < numberOfLines; i++)
      {
        curvePointsForEdges.Add(
          CreateCurvedEdgeGameObject()
        );
        currentRotation += rotation;
      }

      return curvePointsForEdges;

      IList<Vector3> CreateCurvedEdgeGameObject()
      {
        var rotatedPoint = middlePoint +
                           (Quaternion.AngleAxis(currentRotation, rotationAxis) * normalVector * CurvePointDistance);
        var curvePoints = bezierCurveFactory.BezierCurve(startingPoint, rotatedPoint, endingPoint);
        if (debug)
          debugSphereFactory.AddDebugSphere(rotatedPoint);
        return curvePoints;
      }
    }
  }
}
