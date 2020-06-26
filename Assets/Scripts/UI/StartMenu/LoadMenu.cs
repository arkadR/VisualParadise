using System.IO;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.StartMenu
{
  public class LoadMenu : MonoBehaviour
  {
    public GameObject buttonParent;
    public GameObject uiButtonPrefab;

    void Start()
    {
      var files = Directory.GetFiles(Constants.GraphFolder, "*.json");
      for (var i = 0; i < files.Length; i++)
      {
        var filePath = files[i];
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var button = Instantiate(uiButtonPrefab, buttonParent.transform);
        button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
        button.GetComponentInChildren<Text>().text = fileName;
        button.GetComponent<Button>().onClick.AddListener(() => OnClick(filePath));
      }
    }

    void OnClick(string filePath)
    {
      PlayerPrefs.SetString(Constants.GraphFilePathKey, filePath);
      SceneManager.LoadScene(Constants.GameScene, new LoadSceneParameters());
    }
  }
}
