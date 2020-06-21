using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Edges
{
  public class EdgeBuilder : IEdgeBuilder
  {
    const float c_edgeThicknessMultiplier = 0.1f;

    private readonly Func<GameObject, GameObject> _instantiateMethod;
    private readonly MaterialCache _materialCache;
    private readonly LineFactory _lineFactory;
    private List<Vector3> _intermediatePoints;
    private Vector3? _startingNode = null;
    private Vector3? _endingNode = null;
    private float? _rotation = null;
    private GraphElementClass _class;
    private GameObject _lineStart;
    private GameObject _lineEnd;
    private GameObject _label;

    public EdgeBuilder(MaterialCache materialCache, Func<GameObject, GameObject> instantiateMethod)
    {
      _materialCache = materialCache;
      _lineFactory = new LineFactory();
      _instantiateMethod = instantiateMethod;
    }

    public IEdgeBuilder BetweenNodes(Node node1, Node node2)
    {
      _startingNode = node1.Position;
      _endingNode = node2.Position;
      return this;
    }

    public IEdgeBuilder OnOneNode(Node node)
    {
      _startingNode = node.Position;
      _endingNode = node.Position;
      return this;
    }

    public IEdgeBuilder Curved(float rotation)
    {
      _rotation = rotation;
      return this;
    }

    public IEdgeBuilder WithLabel(Func<GameObject> labelFactory, string label, bool visibility)
    {
      _label = labelFactory();
      _label.GetComponentInChildren<Text>().text = label;
      _label.GetComponentInChildren<Text>().enabled = visibility;
      return this;
    }

    public IEdgeBuilder WithClass(GraphElementClass @class)
    {
      _class = @class;
      return this;
    }

    public IEdgeBuilder WithStartLineClass(GraphElementClass @class)
    {
      if (@class?.LineEndingPrefab != null)
      {
        _lineStart = _instantiateMethod(@class.LineEndingPrefab);
        var material = _materialCache.GetByTexturePath(@class.TexturePath);
        var renderers = _lineStart.GetComponentsInChildren<Renderer>()
          .Union(_lineStart.GetComponents<Renderer>());
        foreach (var renderer in renderers)
        {
          renderer.material = material;
        }
      }
      return this;
    }

    public IEdgeBuilder WithEndLineClass(GraphElementClass @class)
    {
      if (@class?.LineEndingPrefab != null)
      {
        _lineEnd = _instantiateMethod(@class.LineEndingPrefab);
        var material = _materialCache.GetByTexturePath(@class.TexturePath);
        var renderers = _lineEnd.GetComponentsInChildren<Renderer>()
          .Union(_lineEnd.GetComponents<Renderer>());
        foreach (var renderer in renderers)
        {
          renderer.material = material;
        }
      }
      return this;
    }

    public void BuildOn(Edge edge)
    {
      if (_startingNode == null || _endingNode == null)
        throw new InvalidOperationException("Provide nodes to build on first");

      edge.segmentGroup?.Destroy();

      _intermediatePoints = _lineFactory.GetIntermediatePoints(_startingNode.Value, _endingNode.Value, _rotation);

      var segments = new List<GameObject>();
      if (_lineStart == null)
      {
        _lineStart = CreateCylinder(_class?.Scale ?? 1f);
        var material = _materialCache.GetByTexturePath(edge.EdgeClass?.TexturePath);
        _lineStart.GetComponent<Renderer>().material = material;
      }
      if (_lineEnd == null)
      {
        _lineEnd = CreateCylinder(_class?.Scale ?? 1f);
        var material = _materialCache.GetByTexturePath(edge.EdgeClass?.TexturePath);
        _lineEnd.GetComponent<Renderer>().material = material;
      }
      for (int i = 1; i < _intermediatePoints.Count - 2; i++)
      {
        var segment = CreateCylinder(_class?.Scale ?? 1f);
        var material = _materialCache.GetByTexturePath(edge.EdgeClass?.TexturePath);
        segment.GetComponent<Renderer>().material = material;
        segments.Add(segment);
      }

      var segmentGroup = new SegmentGroup(_lineStart, segments, _lineEnd);
      segmentGroup.PlaceAlongPoints(_intermediatePoints);
      edge.segmentGroup = segmentGroup;
      edge.labelGameObject = _label;
    }

    private GameObject CreateCylinder(float thickness)
    {
      var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
      cylinder.transform.localScale = new Vector3(
        c_edgeThicknessMultiplier * thickness,
        1,
        c_edgeThicknessMultiplier * thickness);
      return cylinder;
    }
  }
}
