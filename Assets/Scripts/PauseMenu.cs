using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
  public class PauseMenu : MonoBehaviour
  {
    public UnityEngine.GameObject pauseMenu;
    private GraphService graphService;

    void Start()
    {
      pauseMenu.SetActive(false);

      graphService = FindObjectOfType<GraphService>();
    }

    public void SaveButton_OnClick()
    {
      var filePath = PlayerPrefs.GetString(Constants.GraphFilePathKey);
      var graph = graphService.Graph;
      var json = JsonUtility.ToJson(graph);
      File.WriteAllText(filePath, json);
    }

    public void ResumeButton_OnClick()
    {
      GameService.Instance.GlobalUnPauseGame();
    }

    public void QuitButton_OnClick()
    {
      SceneManager.LoadScene(Constants.MainMenuScene);
    }
  }
}
