using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class GraphArrangerTool : ITool
  {
    private readonly GraphArranger _graphArranger;

    public GraphArrangerTool(GraphArranger graphArranger)
    {
      this._graphArranger = graphArranger;
    }

    public string ToolName => "Arrange";
    public bool CanInteractWith(RaycastHit hitInfo) => false;

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      _graphArranger.ToggleArrangement();
    }

    public void OnRightClick(bool isRayCastHit, RaycastHit raycastHit)
    {
      _graphArranger.ToggleReverse();
    }
  }
}
