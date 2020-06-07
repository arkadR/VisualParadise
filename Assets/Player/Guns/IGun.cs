using UnityEngine;

namespace Player.Guns
{
  public interface IGun
  {
    string GetGunName();
    void OnMoveDown(Transform playerTransform, Camera camera);
    void OnSwitchedAway();
  }
}