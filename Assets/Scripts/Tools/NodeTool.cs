using Assets.Scripts.Canvas;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class NodeTool : MonoBehaviour, ITool
  {
    GraphService _graphService;

    public void Start() => _graphService = FindObjectOfType<GraphService>();

    public string ToolName => "Node";
    
    public bool CanInteractWith(RaycastHit hitInfo) => _graphService.IsNode(hitInfo.collider.gameObject);

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      var position = cameraTransform.position + (cameraTransform.forward * 3);

      _graphService.AddNode(position, cameraTransform.rotation);
    }

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      if (isRayCastHit == false)
        return;

      var node = raycastHit.collider.gameObject;
      var contextMenuHandler = FindObjectOfType<ContextMenuHandler>();
      contextMenuHandler.OpenContextMenu(node);
    }
  }
}
