using UnityEngine;

namespace Player.Guns
{
    public class MovementExecutorGun : IGun
    {
        private readonly MovementExecutor movementExecutor;

        public MovementExecutorGun(MovementExecutor movementExecutor)
        {
            this.movementExecutor = movementExecutor;
        }

        public void OnMoveDown(Transform playerTransform, Camera camera)
        {
            movementExecutor.HandleExecuteMovementButtonPress();
        }

        public void OnSwitchedAway()
        {
            //noop
        }
    }
}