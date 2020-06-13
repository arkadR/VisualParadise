using UnityEngine;

namespace Assets.Scripts.Tools
{
  internal class LabelVisibilityTool : ITool
  {
    readonly GraphService _graphService;

    public string ToolName => "Label visibility";

    public LabelVisibilityTool(GraphService graphService)
    {
      _graphService = graphService;
    }

    public bool CanInteractWith(RaycastHit hitInfo) => false;

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit) =>
      _graphService.ToggleLabelVisibility();

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit) { }
  }
}
