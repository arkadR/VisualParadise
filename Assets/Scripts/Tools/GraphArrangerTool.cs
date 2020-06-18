using Assets.Scripts.Common.Extensions;
using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class GraphArrangerTool : ITool, IMovementTool
  {
    readonly GraphArranger _graphArranger;
    readonly ToolPanelController _toolPanelController;

    public GraphArrangerTool(GraphArranger graphArranger, ToolPanelController toolPanelController)
    {
      _graphArranger = graphArranger;
      _toolPanelController = toolPanelController;
    }

    public void Disable() => _graphArranger.DisableArrangement();

    public string ToolName => "Arrange";

    private void RefreshToolgunModeText() => _toolPanelController.SetToolgunModeInfoText(GetArrangerModeLabel());

    public bool CanInteractWith(RaycastHit hitInfo) => false;
    public void UpdateRaycast(bool isHit, RaycastHit hitInfo) { }

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      _graphArranger.ToggleArrangement();
      _toolPanelController.SetBackgroundColor(GetPanelColor());
    }

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      _graphArranger.ToggleMode();
      RefreshToolgunModeText();
    }

    public void OnLeftMouseButtonHeld(Transform cameraTransform) { }

    public void OnLeftMouseButtonReleased() { }

    public void OnSelect()
    {
      _toolPanelController.SetBackgroundColor(GetPanelColor());
      RefreshToolgunModeText();
    }

    private Color GetPanelColor() => _graphArranger.ArrangeEnabled ? Color.green : Color.red;

    private string GetArrangerModeLabel() => _graphArranger.ArrangeMode.GetDescription();
  }
}
