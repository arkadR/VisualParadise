using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  public GameObject pauseMenu;
  private GraphLoader graphLoader;
  private GraphService graphService;

  void Start()
  {
    pauseMenu.SetActive(false);

    graphLoader = FindObjectOfType<GraphLoader>();
    graphService = FindObjectOfType<GraphService>();
  }

  public void SaveButton_OnClick()
  {
    var filePath = PlayerPrefs.GetString("filePath");
    var graph = graphService.Graph;
    
  }

  public void ResumeButton_OnClick()
  {
    GameService.Instance.UnPauseGame();
  }

  public void QuitButton_OnClick()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
