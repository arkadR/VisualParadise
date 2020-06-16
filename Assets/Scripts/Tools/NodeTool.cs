using Assets.Scripts.Canvas;
using Assets.Scripts.Common;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class NodeTool : MonoBehaviour, ITool, IToolChangeObserver
  {
    GraphService _graphService;
    ToolPanelController _toolPanelController;
    private Camera _attachedCamera;
    private bool _shouldGhostNodeBeActive = false;
    private GameObject _ghostNode;
    private const float c_maxInteractDistance = 10f;
    private const float c_addNodeDistance = 4f;

    Node _movedNode = null;
    float _movedNodeDistance = 0f;

    public void Start()
    {
      _ghostNode = GetGhostNode();
      _graphService = FindObjectOfType<GraphService>();
      _toolPanelController = FindObjectOfType<ToolPanelController>();
      _attachedCamera = Camera.main;
      if (_shouldGhostNodeBeActive)
        _ghostNode.SetActive(true);
    }

    public void Update()
    {
      if (!_shouldGhostNodeBeActive)
        return;

      var position = _attachedCamera.transform.position + (_attachedCamera.transform.forward * c_addNodeDistance);
      _ghostNode.transform.position = position;
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
      var position = cameraTransform.position + (cameraTransform.forward * c_addNodeDistance);
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

      _shouldGhostNodeBeActive = false;
      _ghostNode.SetActive(false);
    }

    public void OnLeftMouseButtonReleased()
    {
      _movedNode = null;
      _shouldGhostNodeBeActive = true;
      _ghostNode.SetActive(true);
    }

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      if (!isRayCastHit || raycastHit.collider.gameObject.tag != Constants.PhysicalNodeTag)
        return;

      var node = raycastHit.collider.gameObject;
      var contextMenuHandler = FindObjectOfType<ContextMenuHandler>();
      contextMenuHandler.OpenContextMenu(node);
    }

    public void OnSelect()
    {
      _shouldGhostNodeBeActive = true;
      if (_toolPanelController != null)
        _toolPanelController.SetBackgroundColor(Color.green);
      if (_ghostNode != null)
        _ghostNode.SetActive(true);
    }

    public void OnToolsChanged(ITool newTool)
    {
      _shouldGhostNodeBeActive = false;
      if(_ghostNode != null)
        _ghostNode.SetActive(false);
    }

    private GameObject GetGhostNode()
    {
      NodeGameObjectFactory nodeGameObjectFactory = FindObjectOfType<NodeGameObjectFactory>();
      GameObject ghostNode = nodeGameObjectFactory.CreateGhostNodeGameObject(Vector3.zero, Quaternion.identity);
      ghostNode.SetActive(false);
      return ghostNode;
    }
  }
}
