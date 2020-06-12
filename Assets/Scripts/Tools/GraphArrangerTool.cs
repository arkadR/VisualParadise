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

    public bool CanInteractWith(RaycastHit hitInfo) => false;

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      _graphArranger.ToggleArrangement();
      _toolPanelController.SetBackgroundColor(GetPanelColor());
    }

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit) =>
      _graphArranger.ToggleReverse();

    public void OnSelect() => _toolPanelController.SetBackgroundColor(GetPanelColor());

    private Color GetPanelColor() => _graphArranger.ArrangeEnabled ? Color.green : Color.red;
  }
}
