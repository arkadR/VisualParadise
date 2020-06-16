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
    bool _lastCanInteractWithResult = false;
    private GameObject _ghostNode;
    private const float c_maxInteractDistance = 10f;
    private const float c_addNodeDistance = 4f;

    Node _movedNode = null;
    float _movedNodeDistance = 0f;

    public void Start()
    {
      _ghostNode = CreateGhostNode();
      _graphService = FindObjectOfType<GraphService>();
      _toolPanelController = FindObjectOfType<ToolPanelController>();
      _attachedCamera = Camera.main;
    }

    public void Update()
    {
      if (_movedNode != null || _lastCanInteractWithResult == true)
        _ghostNode.SetActive(false);
      else
      {
        _ghostNode.SetActive(true);
        var position = _attachedCamera.transform.position + (_attachedCamera.transform.forward * c_addNodeDistance);
        _ghostNode.transform.position = position;
      }
    }

    public string ToolName => "Node";
    
    public bool CanInteractWith(RaycastHit hitInfo) => 
      _graphService.IsNode(hitInfo.collider.gameObject) && hitInfo.distance <= c_maxInteractDistance;

    public void UpdateRaycast(bool isHit, RaycastHit hitInfo) => 
      _lastCanInteractWithResult = isHit && CanInteractWith(hitInfo);

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
    }
    
    public void OnLeftMouseButtonReleased()
    {
      _movedNode = null;
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
      enabled = true;
      _toolPanelController?.SetBackgroundColor(Color.green);
      _ghostNode?.SetActive(true);
    }

    public void OnToolsChanged(ITool newTool)
    {
      _ghostNode?.SetActive(false);
      enabled = false;
    }

    private GameObject CreateGhostNode()
    {
      var nodeGameObjectFactory = FindObjectOfType<NodeGameObjectFactory>();
      var ghostNode = nodeGameObjectFactory.CreateGhostNodeGameObject(Vector3.zero, Quaternion.identity);
      ghostNode.SetActive(false);
      return ghostNode;
    }
  }
}
