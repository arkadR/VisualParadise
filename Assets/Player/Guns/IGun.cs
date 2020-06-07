using System.Collections.Generic;
using UnityEngine;
using static GraphLoader;

namespace Player.Guns
{
    public interface IGun
    {
        string GetGunName();
        void OnMoveDown(Transform playerTransform, Camera camera);

        void OnRightClick(Camera camera);

        void OnSwitchedAway();
    }
}