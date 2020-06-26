using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
  public class GameService : MonoBehaviour
  {
    public static GameService Instance;

    PauseMenuBinding _pauseMenu;

    public bool isResumableOnKeyPress = true;

    public bool IsPaused { get; private set; }

    public void Awake()
    {
      if (Instance != null)
        Destroy(Instance);

      Instance = this;

      _pauseMenu = FindObjectOfType<PauseMenuBinding>();

      DontDestroyOnLoad(this);

      Cursor.visible = false;
    }

    public void GlobalPauseGame()
    {
      isResumableOnKeyPress = true;
      _pauseMenu.ShowMenu();
      PauseGame();
    }

    public void GlobalUnPauseGame()
    {
      _pauseMenu.HideMenu();
      UnPauseGame();
    }

    public void PauseGame()
    {
      // gamePanel.SetActive(false);
      IsPaused = true;
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }

    public void UnPauseGame()
    {
      // gamePanel.SetActive(true);
      IsPaused = false;
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    public void PauseGameWithoutResume()
    {
      IsPaused = true;
      isResumableOnKeyPress = false;
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }

    public void UnPauseGameWithoutResume()
    {
      IsPaused = false;
      isResumableOnKeyPress = true;
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
  }
}
