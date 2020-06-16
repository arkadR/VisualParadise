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

    List<(KeyCode keyCode, ITool tool)> _tools;
    public Color activatedColor = Color.green;
    public Image crosshair;
    public float hitDistance = 40f;
    public Color notActivatedColor = Color.white;

    bool _isLeftMouseButtonHeld = false;

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

      var nodeTool = gameObject.AddComponent<NodeTool>();
      var edgeTool = gameObject.AddComponent<EdgeTool>();
      var movementExecutorTool = new MovementExecutorTool(FindObjectOfType<MovementExecutor>(), toolPanelController);
      var graphArrangerTool = new GraphArrangerTool(FindObjectOfType<GraphArranger>(), toolPanelController);
      var labelVisibilityTool = new LabelVisibilityTool(_graphService, toolPanelController);

      _tools = new List<(KeyCode, ITool)>
      {
        (KeyCode.Alpha1, nodeTool),
        (KeyCode.Alpha2, edgeTool),
        (KeyCode.Alpha3, movementExecutorTool),
        (KeyCode.Alpha4, graphArrangerTool),
        (KeyCode.Alpha5, labelVisibilityTool)
      };

      _toolChangeObservers = new HashSet<IToolChangeObserver> { toolPanelController, edgeTool, nodeTool };

      ActiveTool = _tools.First().tool;
      ActiveTool.OnSelect();
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
      // Only clicked event, holding down does not call those
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

      // Updated for every frame
      if (Input.GetMouseButton(0))
      {
        ActiveTool.OnLeftMouseButtonHeld(_attachedCamera.transform);
        _isLeftMouseButtonHeld = true;
      }
      else if (_isLeftMouseButtonHeld)
      {
        ActiveTool.OnLeftMouseButtonReleased();
        _isLeftMouseButtonHeld = false;
      }
    }

    void HandleChangeTool()
    {
      var selectedTool = _tools
        .FirstOrDefault(r => Input.GetKeyDown(r.keyCode) == true);

      if (Input.GetAxis("Mouse ScrollWheel") > 0f)
      {
        var currIdx = _tools.FindIndex(t => t.tool == ActiveTool);
        selectedTool = currIdx == _tools.Count - 1 ? _tools.First() : _tools[currIdx + 1];
      }
      else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
      {
        var currIdx = _tools.FindIndex(t => t.tool == ActiveTool);
        selectedTool = currIdx == 0 ? _tools.Last() : _tools[currIdx - 1];
      }

      if (selectedTool == default)
        return;

      if (ActiveTool == selectedTool.tool)
        return;

      if (selectedTool.tool is IMovementTool)
        DisableMovementTools(selectedTool.tool);

      ActiveTool = selectedTool.tool;
      ActiveTool.OnSelect();
    }


    void DisableMovementTools(ITool toolNotToDisable)
    {
      foreach (var (_, tool) in _tools)
      {
        if (tool != toolNotToDisable && tool is IMovementTool movementTool)
          movementTool.Disable();
      }
    }

  }
}
