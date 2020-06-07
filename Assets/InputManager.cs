using UnityEngine;

public class InputManager : MonoBehaviour
{
  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab))
    {
      var isPaused = GameService.Instance.IsPaused;
      if (isPaused)
      {
        GameService.Instance.UnPauseGame();
      }
      else
      {
        GameService.Instance.PauseGame();
      }
    }
  }
}
