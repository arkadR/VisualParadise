using UnityEngine;

namespace Assets.Scripts
{
  public class GameService : MonoBehaviour
  {
    public static GameService Instance;

    public GameObject pauseMenu;
    public GameObject gamePanel;

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
  }
}
