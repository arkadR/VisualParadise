using System.Collections.Generic;
using UnityEngine;
using static GraphLoader;

namespace Player.Guns
{
    public class MovementExecutorGun : IGun
    {
        private readonly MovementExecutor movementExecutor;

        public string GetGunName() => "Move";

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

        public void OnRightClick(Camera camera)
        {
            
        }
    }
}