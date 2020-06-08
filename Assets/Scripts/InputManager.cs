using UnityEngine;

namespace Assets.Scripts
{
  public class InputManager : MonoBehaviour
  {
    // Update is called once per frame
    void Update()
    {
      if (!Input.GetKeyDown(KeyCode.Tab)) 
        return;
      var isPaused = GameService.Instance.IsPaused;
      var isResumableOnKeyPress = GameService.Instance.isResumableOnKeyPress;
      if (isPaused && isResumableOnKeyPress)
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
