using Assets.Scripts.Canvas;
using Assets.Scripts.Common;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class NodeTool : MonoBehaviour, ITool
  {
    GraphService _graphService;
    ToolPanelController _toolPanelController;
    const float c_maxInteractDistance = 10f;

    Node _movedNode = null;
    float _movedNodeDistance = 0f;

    public void Start()
    {
      _graphService = FindObjectOfType<GraphService>();
      _toolPanelController = FindObjectOfType<ToolPanelController>();
    }

    public string ToolName => "Node";
    
    public bool CanInteractWith(RaycastHit hitInfo) => _graphService.IsNode(hitInfo.collider.gameObject) && hitInfo.distance <= c_maxInteractDistance;

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      if (isRayCastHit == false)
        AddNode(cameraTransform);
      else if (raycastHit.distance > c_maxInteractDistance)
        AddNode(cameraTransform);
      else
        StartNodeInteraction(raycastHit.distance, raycastHit.collider.gameObject);
    }

    void AddNode(Transform cameraTransform)
    {
      var position = cameraTransform.position + (cameraTransform.forward * 3);
      _graphService.AddNode(position, cameraTransform.rotation);
    }

    void StartNodeInteraction(float distance, GameObject nodeGameObject)
    {
      _movedNode = _graphService.FindNodeByGameObject(nodeGameObject);
      _movedNodeDistance = distance;

    }

    public void OnLeftMouseButtonHeld(Transform cameraTransform)
    {
      if (_movedNode == null)
        return;

      var position = cameraTransform.position + (cameraTransform.forward * _movedNodeDistance);
      _movedNode.Position = position;
      _graphService.FixEdgesOfNode(_movedNode);
    }

    public void OnLeftMouseButtonReleased()
    {
      _movedNode = null;
    }



    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      if (!isRayCastHit || raycastHit.collider.gameObject.tag != Constants.PhysicalNodeTag)
        return;

      var node = raycastHit.collider.gameObject;
      var contextMenuHandler = FindObjectOfType<ContextMenuHandler>();
      contextMenuHandler.OpenContextMenu(node);
    }

    public void OnSelect() => _toolPanelController.SetBackgroundColor(Color.green);
  }
}
