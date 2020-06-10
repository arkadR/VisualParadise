using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class MovementExecutorTool : ITool, IMovementTool
  {
    readonly MovementExecutor _movementExecutor;

    public MovementExecutorTool(MovementExecutor movementExecutor)
    {
      _movementExecutor = movementExecutor;
    }

    public void Disable() => _movementExecutor.DisableMovement();

    public string ToolName => "Move";

    public bool CanInteractWith(RaycastHit hitInfo) => false;

    public void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit) =>
      _movementExecutor.ToggleMovement();

    public void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit) =>
      _movementExecutor.ToggleReverse();
  }
}
