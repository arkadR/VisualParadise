using UnityEngine;

namespace Assets.Scripts
{
  public class GameService : MonoBehaviour
  {
    public static GameService Instance;

    public UnityEngine.GameObject pauseMenu;
    public UnityEngine.GameObject gamePanel;

    void Awake()
    {
      if (Instance != null)
        Destroy(Instance);

      Instance = this;

      DontDestroyOnLoad(this);
    }

    public bool IsPaused { get; private set; } = false;

    public bool isResumableOnKeyPress = true;

    public void GlobalPauseGame()
    {
      isResumableOnKeyPress = true;
      pauseMenu.SetActive(true);
      PauseGame();
    }

    public void GlobalUnPauseGame()
    {
      pauseMenu.SetActive(false);
      UnPauseGame();
    }

    public void PauseGame()
    {
      gamePanel.SetActive(false);
      IsPaused = true;
      Cursor.lockState = CursorLockMode.None;
    }

    public void UnPauseGame()
    {
      gamePanel.SetActive(true);
      IsPaused = false;
      Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGameWithoutResume()
    {
      gamePanel.SetActive(false);
      IsPaused = true;
      isResumableOnKeyPress = false;
      Cursor.lockState = CursorLockMode.None;
    }

    public void UnPauseGameWithoutResume()
    {
      gamePanel.SetActive(true);
      IsPaused = false;
      isResumableOnKeyPress = true;
      Cursor.lockState = CursorLockMode.Locked;
    }
  }
}
