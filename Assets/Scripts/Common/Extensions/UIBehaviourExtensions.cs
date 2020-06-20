using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Common.Extensions
{
  public static class UIBehaviourExtensions
  {
    /// <summary>
    /// Sets the center of the UI element at the given Position.
    /// If the z value is negative (behind screen), this will render it off screen.
    /// </summary>
    /// <param Name="uiElement"></param>
    /// <param Name="position"></param>
    public static void SetPositionOnScreen(this UIBehaviour uiElement, Vector3 position)
    {
      uiElement.transform.position = position.z < 0 
        ? Vector3.one * -200 //far outside screen
        : position;
    }
  }
}
