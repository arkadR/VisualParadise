using System.IO;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts
{
  public class PauseMenu : MonoBehaviour
  {
    GraphService graphService;
    public GameObject pauseMenu;
    public GameObject saveBeforeQuitMenu;

    public void Start()
    {
      pauseMenu.SetActive(false);
      saveBeforeQuitMenu.SetActive(false);
      graphService = FindObjectOfType<GraphService>();
    }

    public void SaveButton_OnClick()
    {
      var filePath = PlayerPrefs.GetString(Constants.GraphFilePathKey);
      var graph = graphService.Graph;
      var json = JsonUtility.ToJson(graph);
      File.WriteAllText(filePath, json);
      Toast.Instance.Show("Graph saved succesfully!", 4f, Toast.ToastColor.Green);
    }

    public void ResumeButton_OnClick() => GameService.Instance.GlobalUnPauseGame();

    public void QuitButton_OnClick()
    {
      pauseMenu.SetActive(false);
      saveBeforeQuitMenu.SetActive(true);
    }
  }
}
