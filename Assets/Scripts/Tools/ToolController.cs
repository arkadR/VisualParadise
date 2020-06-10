using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tools
{
  public class ToolController : MonoBehaviour
  {
    private ITool _activeTool;
    private Camera _attachedCamera;
    private GraphService _graphService;
    private ISet<IToolChangeObserver> _toolChangeObservers;

    private IDictionary<KeyCode, ITool> _tools;
    [SerializeField] private Color activatedColor = Color.yellow;
    [SerializeField] private Image crosshair;
    [SerializeField] private float hitDistance = 40f;
    [SerializeField] private Color notActivatedColor = Color.white;

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

    private void Start()
    {
      _attachedCamera = Camera.main;
      _graphService = FindObjectOfType<GraphService>();

      var toolGunController = FindObjectOfType<ToolTextPanelController>();
      var edgeTool = new EdgeTool(_graphService);

      _tools = new Dictionary<KeyCode, ITool>
      {
        [KeyCode.Alpha1] = gameObject.AddComponent<NodeTool>(),
        [KeyCode.Alpha2] = edgeTool,
        [KeyCode.Alpha3] = new MovementExecutorTool(FindObjectOfType<MovementExecutor>()),
        [KeyCode.Alpha4] = new GraphArrangerTool(FindObjectOfType<GraphArranger>())
      };

      _toolChangeObservers = new HashSet<IToolChangeObserver> {toolGunController, edgeTool};

      ActiveTool = _tools.First().Value;
    }

    private void Update()
    {
      if (GameService.Instance.IsPaused)
      {
        return;
      }

      var isHit = RayCast(out var raycastHit);
      crosshair.color = isHit && ActiveTool.CanInteractWith(raycastHit) ? activatedColor : notActivatedColor;
      HandleChangeTool();
      HandleMouseClick(isHit, raycastHit);
    }

    private bool RayCast(out RaycastHit raycastHit)
    {
      var ray = _attachedCamera.ScreenPointToRay(Input.mousePosition);
      return Physics.Raycast(ray, out raycastHit, hitDistance);
    }

    private void HandleMouseClick(bool isHit, RaycastHit raycastHit)
    {
      if (Input.GetButtonDown("Fire1"))
      {
        ActiveTool.OnLeftClick(_attachedCamera.transform, isHit, raycastHit);
      }
      else if (Input.GetButtonDown("Fire2"))
      {
        ActiveTool.OnRightClick(_attachedCamera.transform, isHit, raycastHit);
      }
    }

    private void HandleChangeTool()
    {
      var pressedGunKey = _tools
        .Keys
        .FirstOrDefault(Input.GetKeyDown);

      if (pressedGunKey == KeyCode.None)
      {
        return;
      }

      if (ActiveTool == _tools[pressedGunKey])
      {
        return;
      }

      ActiveTool = _tools[pressedGunKey];
    }
  }
}
