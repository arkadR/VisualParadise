using System.IO;
using Assets.Scripts.Common;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
  public class PauseMenuBinding : MonoBehaviour
  {
    bool _isSaved;
    GraphService graphService;
    public GameObject pauseMenu;
    public GameObject saveDialog;
    public GameObject settingsMenu;

    public void Start()
    {
      pauseMenu.SetActive(false);
      saveDialog.SetActive(false);
      settingsMenu.SetActive(false);
      graphService = FindObjectOfType<GraphService>();
    }

    public void ResumeButton_OnClick() => GameService.Instance.GlobalUnPauseGame();

    public void SaveButton_OnClick() => Save();

    public void SettingsButton_OnClick()
    {
      pauseMenu.SetActive(false);
      settingsMenu.SetActive(true);
    }

    public void SettingsMenuBack_OnClick()
    {
      settingsMenu.SetActive(false);
      pauseMenu.SetActive(true);
    }

    public void QuitButton_OnClick()
    {
      if (_isSaved)
        Quit();
      else
      {
        pauseMenu.SetActive(false);
        saveDialog.SetActive(true);
      }
    }

    public void SaveDialogYes_OnClick()
    {
      Save();
      Quit();
    }

    public void SaveDialogNo_OnClick() => Quit();

    public void SaveDialogCancel_OnClick()
    {
      pauseMenu.SetActive(true);
      saveDialog.SetActive(false);
    }

    public void ShowMenu()
    {
      _isSaved = false;
      pauseMenu.SetActive(true);
    }

    public void HideMenu()
    {
      pauseMenu.SetActive(false);
      saveDialog.SetActive(false);
    }

    void Quit() => SceneManager.LoadScene(Constants.MainMenuScene);

    void Save()
    {
      var filePath = PlayerPrefs.GetString(Constants.GraphFilePathKey);
      var graph = graphService.Graph;
      //var json = JsonUtility.ToJson(graph, true);
      var json = JsonConvert.SerializeObject(graph);
      File.WriteAllText(filePath, json);
      Toast.Instance.Show("Graph saved successfully!", 4f, Toast.ToastColor.Green);
      _isSaved = true;
    }
  }
}
