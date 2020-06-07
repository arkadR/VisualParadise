using UnityEngine;

public class GameService : MonoBehaviour
{
  public static GameService Instance;

  public GameObject pauseMenu;
  public GameObject gamePanel;

  void Awake()
  {
    if (Instance != null)
      GameObject.Destroy(Instance);
      
    Instance = this;

    DontDestroyOnLoad(this);
  }

  public bool IsPaused { get; private set; } = false;
  public void PauseGame()
  {
    pauseMenu.SetActive(true);
    gamePanel.SetActive(false);
    IsPaused = true;
    Cursor.lockState = CursorLockMode.None;
  }

  public void UnPauseGame()
  {
    pauseMenu.SetActive(false);
    gamePanel.SetActive(true);
    IsPaused = false;
    Cursor.lockState = CursorLockMode.Locked;
  }
}
