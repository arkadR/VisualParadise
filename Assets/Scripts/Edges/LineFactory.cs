using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.Common.Utils;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public class LineFactory 
  {
    const int c_singleNodeEdgeCurveStrength = 3;
    const int c_linePointsForSingleNodeEdge = 30;
    const int c_linePointsForNormalEdge = 15;
    public BezierCurveFactory bezierCurveFactory = new BezierCurveFactory();
    public bool debug;
    public DebugSphereFactory debugSphereFactory;

    public List<Vector3> GetIntermediatePoints(
      Vector3 startingPoint,
      Vector3 endingPoint,
      float? rotation = null,
      int numOfPoints = c_linePointsForNormalEdge)
    {
      if (startingPoint == endingPoint)
      {
        return GetPointsForEdgeOnOneNode(startingPoint, rotation ?? 0f, numOfPoints);
      }

      if (rotation != null)
        return GetPointsForCurvedEdge(startingPoint, endingPoint, rotation.Value, numOfPoints);

      return GetPointsForStraightLine(startingPoint, endingPoint, numOfPoints);
    }

    private List<Vector3> GetPointsForEdgeOnOneNode(Vector3 point, float rotation, int numOfPoints)
    {
      Debug.Log(rotation);

      var centerVector = Vector3.up * c_singleNodeEdgeCurveStrength;
      var v1 = Quaternion.Euler(60, 0, 0) * centerVector;
      var v2 = Quaternion.Euler(-60, 0, 0) * centerVector;

      var points = bezierCurveFactory.BezierCurve4(
        point,
        point + (Quaternion.Euler(0, 0, rotation) * v1),
        point + (Quaternion.Euler(0, 0, rotation) * v2),
        point,
        numOfPoints);

      return points;
    }

    private List<Vector3> GetPointsForCurvedEdge(
      Vector3 startingPoint, 
      Vector3 endingPoint, 
      float rotation,
      int numOfPoints)
    {
      var middlePoint = (endingPoint + startingPoint) / 2;
      var rotationAxis = (endingPoint - startingPoint).normalized;
      var normalVector = Vector3.Cross(Vector3.up, rotationAxis).normalized;

      var curvePointDistance = (endingPoint - startingPoint).magnitude / 3;
      var rotatedPoint = middlePoint +
                         (Quaternion.AngleAxis(rotation, rotationAxis) * normalVector * curvePointDistance);
      var curvePoints = bezierCurveFactory.BezierCurve3(
        startingPoint,
        rotatedPoint,
        endingPoint,
        numOfPoints);

      return curvePoints;
    }


    private List<Vector3> GetPointsForStraightLine(Vector3 startingPoint, Vector3 endingPoint, int numOfPoints)
    {
      var directionVector = endingPoint - startingPoint;
      var points = new List<Vector3>();
      for (int i = 0; i < numOfPoints; i++)
      {
        points.Add(startingPoint + directionVector * ((float)i/(numOfPoints - 1)));
      }
      return points;
    }
  }
}
