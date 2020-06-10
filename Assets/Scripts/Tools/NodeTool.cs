using Assets.Scripts.Canvas;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class NodeTool : MonoBehaviour, ITool
  {
    private GraphService _graphService;

    public string ToolName => "Node";

    public bool CanInteractWith(RaycastHit hitInfo)
    {
      return _graphService.IsNode(hitInfo.collider.gameObject);
    }

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      var position = cameraTransform.position + (cameraTransform.forward * 3);

      _graphService.AddNode(position, cameraTransform.rotation);
    }

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      if (!isRayCastHit)
      {
        return;
      }

      var node = raycastHit.collider.gameObject;
      var contextMenuHandler = FindObjectOfType<ContextMenuHandler>();
      contextMenuHandler.OpenContextMenu(node);
    }

    private void Start()
    {
      _graphService = FindObjectOfType<GraphService>();
    }
  }
}
