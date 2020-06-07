using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
  public GameObject buttonParent;
  public GameObject uiButtonPrefab;

  private const int c_buttonGap = 30;

  void Start()
  {
    var files = Directory.GetFiles("savedgraphs/", "*.json");
    for (var i = 0; i < files.Length; i++)
    {
      var filePath = files[i];
      var fileName = Path.GetFileNameWithoutExtension(filePath);
      var button = Instantiate(uiButtonPrefab, buttonParent.transform);
      // button.transform.localPosition = new Vector3(0, - (i+1) * c_buttonGap, 0);
      button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
      button.GetComponentInChildren<Text>().text = fileName;
      button.GetComponent<Button>().onClick.AddListener(() => OnClick(filePath));
    }
  }

  private void OnClick(string filePath)
  {

  }
}
