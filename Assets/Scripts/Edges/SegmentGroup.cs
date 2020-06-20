using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common.Extensions;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public class SegmentGroup
  {
    readonly GameObject[] _segments;
    const float c_segmentAdditionalLength = 0.01f;

    public SegmentGroup(GameObject[] segments)
    {
      if (segments == null || segments.Length < 1)
        throw new ArgumentException("At least one segment has To be provided");

      _segments = segments;
    }

    public GameObject MiddleSegment => _segments[_segments.Length / 2];


    public void PlaceAlongPoints(IList<Vector3> points)
    {
      if (points.Count - 1 != _segments.Length)
        throw new ArgumentException("Number of _segments and provided points don't match up");

      // TODO: Consider optimizing
      var pointsWithoutFirst = points.Skip(1);
      var pointsWithoutLast = points.SkipLast(1);
      var pointPairs = pointsWithoutFirst.Zip(pointsWithoutLast, (p1, p2) => (p1, p2)).ToList();
      for (var i = 0; i < _segments.Length; i++)
      {
        var (p1, p2) = pointPairs[i];
        PlaceBetweenPoints(_segments[i], p1, p2);
      }
    }

    public bool Contains(GameObject gameObject) => _segments.Contains(gameObject);

    public void Destroy()
    {
      foreach (var gameObject in _segments)
      {
        UnityEngine.Object.Destroy(gameObject);
      }
    }

    private void PlaceBetweenPoints(GameObject segment, Vector3 point1, Vector3 point2)
    {
      var direction = point2 - point1;
      var distance = direction.magnitude;
      var (scaleX, scaleY, scaleZ) = segment.transform.localScale;

      segment.transform.localScale = new Vector3(scaleX, distance / 2 + c_segmentAdditionalLength, scaleZ);
      segment.transform.position = (point1 + point2) / 2;
      segment.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }
  }
}
