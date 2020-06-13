using System.IO;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class NewGraphMenu : MonoBehaviour
  {
    public Button CreateButton;
    public InputField GraphNameInput;

    public void Start() => CreateButton.interactable = false;

    public void Update()
    {
      //TODO: Consider better validation
      var isNameValid = !string.IsNullOrEmpty(GraphNameInput.text);
      CreateButton.interactable = isNameValid;
    }

    public void StartNew()
    {
      var graphName = GraphNameInput.text;
      var filePath = $"{Constants.GraphFolder}{graphName}.json";
      File.Create(filePath);
      PlayerPrefs.SetString(Constants.GraphFilePathKey, filePath);
      SceneManager.LoadScene(Constants.GameScene);
    }
  }
}
