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
    ISet<IToolChangeObserver> _toolChangeObservers;

    IDictionary<KeyCode, ITool> _tools;
    [SerializeField] Color activatedColor = Color.yellow;
    [SerializeField] Image crosshair;
    [SerializeField] float hitDistance = 40f;
    [SerializeField] Color notActivatedColor = Color.white;

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

      var toolGunController = FindObjectOfType<ToolTextPanelController>();
      var edgeTool = new EdgeTool(_graphService);
      var labelVisibilityTool = new LabelVisibilityTool(_graphService);

      _tools = new Dictionary<KeyCode, ITool>
      {
        [KeyCode.Alpha1] = gameObject.AddComponent<NodeTool>(),
        [KeyCode.Alpha2] = edgeTool,
        [KeyCode.Alpha3] = new MovementExecutorTool(FindObjectOfType<MovementExecutor>()),
        [KeyCode.Alpha4] = new GraphArrangerTool(FindObjectOfType<GraphArranger>()),
        [KeyCode.Alpha5] = labelVisibilityTool
      };

      _toolChangeObservers = new HashSet<IToolChangeObserver> {toolGunController, edgeTool};

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
        ActiveTool.OnLeftClick(_attachedCamera.transform, isHit, raycastHit);
      else if (Input.GetButtonDown("Fire2"))
        ActiveTool.OnRightClick(_attachedCamera.transform, isHit, raycastHit);
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
