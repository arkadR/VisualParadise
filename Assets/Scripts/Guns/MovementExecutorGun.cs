using UnityEngine;

namespace Assets.Scripts.Guns
{
  public class MovementExecutorGun : IGun
  {
    private readonly MovementExecutor movementExecutor;

    public string GunName => "Move";

    public MovementExecutorGun(MovementExecutor movementExecutor)
    {
      this.movementExecutor = movementExecutor;
    }

    public void OnMoveDown(Transform playerTransform, Camera camera)
    {
      movementExecutor.ToggleMovement();
    }

    public void OnSwitchedAway()
    {
      //noop
    }

    public void OnRightClick(Camera camera)
    {
      movementExecutor.ToggleReverse();
    }
  }
}
