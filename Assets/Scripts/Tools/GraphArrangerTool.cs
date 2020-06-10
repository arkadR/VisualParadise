using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class GraphArrangerTool : ITool, IMovementTool
  {
    readonly GraphArranger _graphArranger;

    public GraphArrangerTool(GraphArranger graphArranger)
    {
      _graphArranger = graphArranger;
    }

    public void Disable() => _graphArranger.DisableArrangement();

    public string ToolName => "Arrange";

    public bool CanInteractWith(RaycastHit hitInfo) => false;

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit) =>
      _graphArranger.ToggleArrangement();

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit) =>
      _graphArranger.ToggleReverse();
  }
}
