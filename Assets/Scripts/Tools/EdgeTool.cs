using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  enum EdgeMode { Create, Delete }

  public class EdgeTool : ITool, IToolChangeObserver
  {
    private const float _nodeHitGlowStrength = 0.5f;
    private readonly Color _createGlowColor = Color.green;
    private readonly Color _deleteGlowColor = Color.red;
    private readonly GraphService _graphService;
    private readonly Material _edgeMaterial;
    private EdgeMode _mode = EdgeMode.Create;
    private Node _previouslyHitNode;

    public EdgeTool(GraphService graphService, Material edgeMaterial)
    {
      this._graphService = graphService;
      this._edgeMaterial = edgeMaterial;
    }

    private EdgeMode Mode
    {
      get => _mode;
      set
      {
        if (_mode != value)
        {
          ClearPreviouslyHitNode();
          _mode = value;
        }
      }
    }

    public void OnToolsChanged(ITool newTool)
    {
      ClearPreviouslyHitNode();
    }

    public string ToolName => "Edge";

    public bool CanInteractWith(RaycastHit hitInfo) => _graphService.IsNode(hitInfo.collider.gameObject);

    public void OnLeftClick(Transform cameraTransform, bool isHit, RaycastHit raycastHit)
    {
      Mode = EdgeMode.Create;
      ModifyEdge(isHit, raycastHit);
    }

    public void OnRightClick(bool isHit, RaycastHit raycastHit)
    {
      Mode = EdgeMode.Delete;
      ModifyEdge(isHit, raycastHit);
    }

    private void ClearPreviouslyHitNode()
    {
      if (_previouslyHitNode != null)
      {
        _previouslyHitNode.gameObject.DisableGlow();
        _previouslyHitNode = null;
      }
    }

    private void ModifyEdge(bool isHit, RaycastHit raycastHit)
    {
      if (!isHit)
        return;

      var gameObjectHit = raycastHit.collider.gameObject;
      var currentlyHitNode = _graphService.FindNodeByGameObject(gameObjectHit);

      // If not a node, don't do anything
      if (currentlyHitNode == null)
        return;

      // If player hit the same node twice, don't do anything
      if (currentlyHitNode == _previouslyHitNode)
      {
        _previouslyHitNode.gameObject.DisableGlow();
        _previouslyHitNode = null;
        return;
      }

      if (_previouslyHitNode == null)
      {
        _previouslyHitNode = currentlyHitNode;
        _previouslyHitNode.gameObject.EnableGlow();
        var glowColor = Mode == EdgeMode.Create ? _createGlowColor : _deleteGlowColor;
        _previouslyHitNode.gameObject.SetGlow(glowColor * _nodeHitGlowStrength);
        return;
      }

      var existingEdge = _graphService.FindEdgeByNodes(_previouslyHitNode, currentlyHitNode);

      switch (Mode)
      {
        case EdgeMode.Create:
        {
          //Don't create duplicate edge
          if (existingEdge != null)
            return;

          _graphService.AddEdge(currentlyHitNode, _previouslyHitNode, _edgeMaterial);
          break;
        }
        case EdgeMode.Delete:
        {
          if (existingEdge == null)
            return;
          _graphService.RemoveEdge(existingEdge);
          break;
        }
      }

      _previouslyHitNode.gameObject.DisableGlow();
      _previouslyHitNode = null;
    }
  }
}
