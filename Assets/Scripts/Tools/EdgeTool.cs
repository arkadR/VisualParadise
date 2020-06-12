using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  enum EdgeMode { Create, Delete }

  public class EdgeTool : ITool, IToolChangeObserver
  {
    const float _nodeHitGlowStrength = 0.5f;
    readonly Color _createGlowColor = Color.green;
    readonly Color _deleteGlowColor = Color.red;
    readonly GraphService _graphService;
    readonly ToolPanelController _toolPanelController;
    EdgeMode _mode = EdgeMode.Create;
    Node _previouslyHitNode;

    public EdgeTool(GraphService graphService, ToolPanelController toolPanelController)
    {
      _graphService = graphService;
      _toolPanelController = toolPanelController;
    }

    EdgeMode Mode
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

    public string ToolName => "Edge";

    public bool CanInteractWith(RaycastHit hitInfo) => _graphService.IsNode(hitInfo.collider.gameObject);

    public void OnLeftClick(Transform cameraTransform, bool isHit, RaycastHit raycastHit)
    {
      Mode = EdgeMode.Create;
      OnActionPerformed(isHit, raycastHit);
    }

    public void OnRightClick(Transform cameraTransform, bool isHit, RaycastHit raycastHit)
    {
      Mode = EdgeMode.Delete;
      OnActionPerformed(isHit, raycastHit);
    }

    public void OnSelect() => _toolPanelController.SetBackgroundColor(Color.green);

    public void OnToolsChanged(ITool newTool) => ClearPreviouslyHitNode();

    void ClearPreviouslyHitNode()
    {
      if (_previouslyHitNode != null)
      {
        _previouslyHitNode.gameObject.DisableGlow();
        _previouslyHitNode = null;
      }
    }

    void OnActionPerformed(bool isHit, RaycastHit raycastHit)
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

          _graphService.AddEdge(currentlyHitNode, _previouslyHitNode);
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
