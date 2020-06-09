using Assets.Scripts.Canvas;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class NodeTool : MonoBehaviour, ITool
  {
    private GraphService _graphService;
    private Material _nodeMaterial;

    private void Start()
    {
      _nodeMaterial = Resources.Load<Material>("Materials/Node Material");
      _graphService = FindObjectOfType<GraphService>();
    }

    public string ToolName => "Node";

    public bool CanInteractWith(RaycastHit hitInfo) => _graphService.IsNode(hitInfo.collider.gameObject);

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      var position = cameraTransform.position + cameraTransform.forward * 3;

      _graphService.AddNode(position, cameraTransform.rotation, _nodeMaterial);
    }

    public void OnRightClick(bool isRayCastHit, RaycastHit raycastHit)
    {
      if (!isRayCastHit)
        return;

      var node = raycastHit.collider.gameObject;
      var contextMenuHandler = FindObjectOfType<ContextMenuHandler>();
      contextMenuHandler.OpenContextMenu(node);
    }
  }
}
