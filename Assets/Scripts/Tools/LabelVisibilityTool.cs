using UnityEngine;

namespace Assets.Scripts.Tools
{
  internal class LabelVisibilityTool : ITool
  {
    readonly GraphService _graphService;
    bool _visibilityEnabled = false;

    public string ToolName => "Label visibility";

    public LabelVisibilityTool(GraphService graphService)
    {
      _graphService = graphService;
    } 

    public bool CanInteractWith(RaycastHit hitInfo) => false;

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      _visibilityEnabled = !_visibilityEnabled;
      _graphService.SetLabelVisibility(_visibilityEnabled);
    }

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit) { }
  }
}
