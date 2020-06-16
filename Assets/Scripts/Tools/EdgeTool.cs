using Assets.Scripts.Canvas;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  enum EdgeMode { Create, Delete }

  public class EdgeTool : MonoBehaviour, ITool, IToolChangeObserver
  {
    readonly Color _createGlowColor = Color.green;
    readonly float _nodeHitGlowStrength = 0.5f;
    GraphService _graphService;
    ToolPanelController _toolPanelController;
    Node _previouslyHitNode;

    public void Start()
    {
      _graphService = FindObjectOfType<GraphService>();
      _toolPanelController = FindObjectOfType<ToolPanelController>();
    }

    public string ToolName => "Edge";

    public bool CanInteractWith(RaycastHit hitInfo) =>
      _graphService.IsNode(hitInfo.collider.gameObject) || 
      _graphService.IsEdge(hitInfo.collider.gameObject) && 
      _previouslyHitNode == null;

    public void UpdateRaycast(bool isHit, RaycastHit hitInfo) { }

    public void OnLeftClick(Transform cameraTransform, bool isHit, RaycastHit raycastHit)
    {
      if (!isHit || raycastHit.collider.gameObject.tag != Constants.PhysicalNodeTag)
        return;

      OnActionPerformed(isHit, raycastHit);
    }

    public void OnRightClick(Transform cameraTransform, bool isHit, RaycastHit raycastHit)
    {
      if (!isHit || raycastHit.collider.gameObject.tag != Constants.PhysicalEdgeTag)
        return;

      FindObjectOfType<EdgeMenuHandler>().OpenContextMenu(raycastHit.collider.gameObject);
    }

    public void OnLeftMouseButtonHeld(Transform cameraTransform) { }

    public void OnLeftMouseButtonReleased() { }

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

      if (_previouslyHitNode == null)
      {
        _previouslyHitNode = currentlyHitNode;
        _previouslyHitNode.gameObject.EnableGlow();
        var glowColor = _createGlowColor;
        _previouslyHitNode.gameObject.SetGlow(glowColor * _nodeHitGlowStrength);
        return;
      }

      _graphService.AddEdge(currentlyHitNode, _previouslyHitNode);
      _previouslyHitNode.gameObject.DisableGlow();
      _previouslyHitNode = null;
    }
  }
}
