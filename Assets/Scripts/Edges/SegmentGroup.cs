using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Extensions;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public class SegmentGroup
  {
    readonly GameObject[] _segments;
    readonly GameObject _lineStart;
    readonly GameObject _lineEnd;

    const float c_segmentAdditionalLength = 0.01f;

    public SegmentGroup(GameObject lineStart, IEnumerable<GameObject> segments, GameObject lineEnd)
    {
      var segmentArr = segments as GameObject[] ?? segments.ToArray();
      if (segments == null || segmentArr.Any() == false)
        throw new ArgumentException("At least one segment has To be provided");

      if (lineStart == null)
        throw new ArgumentException(nameof(lineStart));

      if (lineEnd == null)
        throw new ArgumentException(nameof(lineEnd));

      _segments = segmentArr.ToArray();
      _lineStart = lineStart;
      _lineEnd = lineEnd;
    }

    public GameObject MiddleSegment => _segments[_segments.Length / 2];


    public void PlaceAlongPoints(IList<Vector3> points)
    {
      if (points.Count - 1 != _segments.Length)
        throw new ArgumentException("Number of _segments and provided points don't match up");

      PlaceBetweenPoints(_lineStart, points[1], points[0]);
      for (int i = 0; i < _segments.Length; i++)
      {
        PlaceBetweenPoints(_segments[i], points[i], points[i+1]);
      }
      PlaceBetweenPoints(_lineEnd, points[points.Count-2], points[points.Count-1]);
    }

    public bool Contains(GameObject gameObject) => _segments.Contains(gameObject);

    public void Destroy()
    {
      foreach (var gameObject in _segments)
      {
        UnityEngine.Object.Destroy(gameObject);
      }
      UnityEngine.Object.Destroy(_lineStart);
      UnityEngine.Object.Destroy(_lineEnd);
    }

    private void PlaceBetweenPoints(GameObject segment, Vector3 point1, Vector3 point2)
    {
      var isCustomEnding = segment.tag == Constants.CustomLineEndingTag;
      var direction = point2 - point1;
      var distance = direction.magnitude;
      var (scaleX, scaleY, scaleZ) = segment.transform.localScale;
      var desiredScaleY = distance / 2 + c_segmentAdditionalLength;

      if (isCustomEnding)
      {
        segment.transform.position = point2;
      }
      else
      {
        segment.transform.localScale = new Vector3(scaleX, desiredScaleY, scaleZ);
        segment.transform.position = (point1 + point2) / 2;
      }

      segment.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }
  }
}
