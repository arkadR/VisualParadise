using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common.Extensions;
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

    public IEdgeBuilder WithLabel(Func<GameObject> labelFactoryMethod, string label, bool visibility)
    {
      _label = labelFactoryMethod();
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
        _lineStart = CreateLineEnding(@class);

      return this;
    }

    public IEdgeBuilder WithEndLineClass(GraphElementClass @class)
    {
      if (@class?.LineEndingPrefab != null)
        _lineEnd = CreateLineEnding(@class); 
      
      return this;
    }

    public void BuildOn(Edge edge)
    {
      if (_startingNode == null || _endingNode == null)
        throw new InvalidOperationException("Provide nodes to build on first");

      edge.segmentGroup?.Destroy();

      _intermediatePoints = _lineFactory.GetIntermediatePoints(_startingNode.Value, _endingNode.Value, _rotation);

      if (_lineStart == null)
        _lineStart = CreateDefaultLineEnding(edge.EdgeClass?.TexturePath);

      if (_lineEnd == null)
        _lineEnd = CreateDefaultLineEnding(edge.EdgeClass?.TexturePath);

      var segments = new List<GameObject>();
      for (int i = 0; i < _intermediatePoints.Count - 1; i++)
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

    private GameObject CreateDefaultLineEnding(string texturePath)
    {
      var lineEnding = CreateCylinder(_class?.Scale ?? 1f);
      var material = _materialCache.GetByTexturePath(texturePath);
      lineEnding.GetComponent<Renderer>().material = material;
      return lineEnding;
    }

    private GameObject CreateLineEnding(GraphElementClass @class)
    {
      var gameObject = _instantiateMethod(@class.LineEndingPrefab);
      var material = _materialCache.GetByTexturePath(@class.TexturePath);
      var renderers = gameObject.GetAllComponentsOfType<Renderer>();
      foreach (var renderer in renderers)
      {
        renderer.material = material;
      }

      if (@class.Scale != null)
      {
        var (scaleX, scaleY, scaleZ) = gameObject.transform.localScale;
        gameObject.transform.localScale
          = new Vector3(scaleX * @class.Scale.Value, scaleY, scaleZ * @class.Scale.Value);
      }

      return gameObject;
    }
  }
}
