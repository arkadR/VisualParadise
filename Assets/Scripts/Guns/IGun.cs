using UnityEngine;

namespace Assets.Scripts.Guns
{
  public interface IGun
  {
    string GunName { get; }
    void OnMoveDown(Transform playerTransform, Camera camera);
    void OnSwitchedAway();
    void OnRightClick(Camera camera);
  }
}
