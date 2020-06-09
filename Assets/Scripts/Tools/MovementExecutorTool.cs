using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class MovementExecutorTool : ITool
  {
    private readonly MovementExecutor _movementExecutor;

    public MovementExecutorTool(MovementExecutor movementExecutor)
    {
      this._movementExecutor = movementExecutor;
    }

    public string ToolName => "Move";
    public bool CanInteractWith(RaycastHit hitInfo) => false;

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit)
    {
      _movementExecutor.ToggleMovement();
    }

    public void OnRightClick(bool isRayCastHit, RaycastHit raycastHit)
    {
      _movementExecutor.ToggleReverse();
    }
  }
}
