using UnityEngine;

namespace Player.Guns
{
    public interface IGun
    {
        void OnMoveDown(Transform playerTransform);

        void OnSwitchedAway();
    }
}