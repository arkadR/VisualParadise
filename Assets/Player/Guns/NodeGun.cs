using Assets.Scripts;
using UnityEngine;

namespace Player.Guns
{
  public class NodeGun : MonoBehaviour, IGun
  {
    private GraphService graphService;
    private Material nodeMaterial;

    private void Start()
    {
        nodeMaterial = Resources.Load<Material>("Materials/Node Material");
        graphService = FindObjectOfType<GraphService>();
    }

    public string GunName => "Node";

    public void OnMoveDown(Transform playerTransform, Camera camera)
    {
      var transform = camera.transform;
      var rotation = transform.rotation;
      var position = transform.position + transform.forward * 3;

      graphService.AddNode(position, rotation, nodeMaterial);
    }
    
    public void OnRightClick(Camera camera)
    {
      var ray = camera.ScreenPointToRay(Input.mousePosition);

      if (!Physics.Raycast(ray, out var hit))
        return;
      var node = hit.collider.gameObject;
      var contextMenuHandler = FindObjectOfType<ContextMenuHandler>();
      contextMenuHandler.OpenContextMenu(graphService.physicalNodes, graphService.physicalEdges, node);
    }

    public void OnSwitchedAway()
    {
      //noop
    }
  }
}