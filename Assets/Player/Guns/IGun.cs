using UnityEngine;

namespace Player.Guns
{
  public interface IGun
  {
    string GunName { get; }
    void OnMoveDown(Transform playerTransform, Camera camera);
    void OnSwitchedAway();
  }
}