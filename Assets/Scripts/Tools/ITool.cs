﻿using UnityEngine;

namespace Assets.Scripts.Tools
{
  public interface ITool
  {
    string ToolName { get; }
    bool CanInteractWith(RaycastHit hitInfo);
    void UpdateRaycast(bool isHit, RaycastHit hitInfo);
    void OnLeftClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit);
    void OnRightClick(Transform cameraTransform, bool isRayCastHit, RaycastHit raycastHit);
    void OnLeftMouseButtonHeld(Transform cameraTransform);
    void OnLeftMouseButtonReleased();
    void OnSelect();
  }
}
