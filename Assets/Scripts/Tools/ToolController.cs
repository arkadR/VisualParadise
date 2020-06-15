using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tools
{
  public class ToolController : MonoBehaviour
  {
    ITool _activeTool;
    Camera _attachedCamera;
    GraphService _graphService;
    ToolgunRecoil _toolgunRecoil;
    ISet<IToolChangeObserver> _toolChangeObservers;

    IDictionary<KeyCode, ITool> _tools;
    public Color activatedColor = Color.yellow;
    public Image crosshair;
    public float hitDistance = 40f;
    public Color notActivatedColor = Color.white;

    public ITool ActiveTool
    {
      get => _activeTool;
      set
      {
        _activeTool = value;
        foreach (var observer in _toolChangeObservers)
        {
          observer.OnToolsChanged(_activeTool);
        }
      }
    }

    void Start()
    {
      _attachedCamera = Camera.main;
      _graphService = FindObjectOfType<GraphService>();
      _toolgunRecoil = FindObjectOfType<ToolgunRecoil>();

      var toolPanelController = FindObjectOfType<ToolPanelController>();
      var labelVisibilityTool = new LabelVisibilityTool(_graphService, toolPanelController);

      _tools = new Dictionary<KeyCode, ITool>
      {
        [KeyCode.Alpha1] = gameObject.AddComponent<NodeTool>(),
        [KeyCode.Alpha2] = gameObject.AddComponent<EdgeTool>(),
        [KeyCode.Alpha3] = new MovementExecutorTool(FindObjectOfType<MovementExecutor>(), toolPanelController),
        [KeyCode.Alpha4] = new GraphArrangerTool(FindObjectOfType<GraphArranger>(), toolPanelController),
        [KeyCode.Alpha5] = labelVisibilityTool
      };

      _toolChangeObservers = new HashSet<IToolChangeObserver> { toolPanelController, gameObject.GetComponent<EdgeTool>() };

      ActiveTool = _tools.First().Value;
    }

    void Update()
    {
      if (GameService.Instance.IsPaused)
        return;

      var isHit = RayCast(out var raycastHit);
      crosshair.color = isHit && ActiveTool.CanInteractWith(raycastHit) ? activatedColor : notActivatedColor;
      HandleChangeTool();
      HandleMouseClick(isHit, raycastHit);
    }

    bool RayCast(out RaycastHit raycastHit)
    {
      var ray = _attachedCamera.ScreenPointToRay(Input.mousePosition);
      return Physics.Raycast(ray, out raycastHit, hitDistance);
    }

    void HandleMouseClick(bool isHit, RaycastHit raycastHit)
    {
      if (Input.GetButtonDown("Fire1"))
      {
        ActiveTool.OnLeftClick(_attachedCamera.transform, isHit, raycastHit);
        _toolgunRecoil.AddRecoil();
      }
      else if (Input.GetButtonDown("Fire2"))
      {
        ActiveTool.OnRightClick(_attachedCamera.transform, isHit, raycastHit);
        _toolgunRecoil.AddRecoil();
      }
    }

    void HandleChangeTool()
    {
      var pressedToolKey = _tools
        .Keys
        .FirstOrDefault(Input.GetKeyDown);

      if (pressedToolKey == KeyCode.None)
        return;

      var newTool = _tools[pressedToolKey];
      if (ActiveTool == newTool)
        return;

      if (newTool is IMovementTool)
        DisableMovementTools(newTool);

      ActiveTool = newTool;
      ActiveTool.OnSelect();
    }

    
    void DisableMovementTools(ITool toolNotToDisable)
    {
      foreach (var tool in _tools.Values)
      {
        if (tool != toolNotToDisable && tool is IMovementTool movementTool)
          movementTool.Disable();
      }
    }

  }
}
