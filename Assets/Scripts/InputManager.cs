using UnityEngine;

namespace Assets.Scripts
{
  public class InputManager : MonoBehaviour
  {
    // Update is called once per frame
    void Update()
    {
      if (!Input.GetKeyDown(KeyCode.Tab) || !GameService.Instance.isResumableOnKeyPress) 
        return;
      
      if (GameService.Instance.IsPaused)
      {
        GameService.Instance.GlobalUnPauseGame();
      }
      else
      {
        GameService.Instance.GlobalPauseGame();;
      }
    }
  }
}
