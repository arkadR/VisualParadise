using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas
{
  public class LabelMenuHandler : MonoBehaviour
  {
    private Edge _edge;

    private GameObject _previousMenu;

    public GameObject labelMenu;

    public InputField labelInput;

    public Button saveButton;

    private bool IsInputCorrect() => labelInput.text.Length > 0;

    public void Start()
    {
      labelInput.onValueChanged.AddListener((_) => new LabelValidator(labelInput).OnValueChanged());
      labelMenu.SetActive(false);
    }

    public void Update()
    {
      saveButton.interactable = IsInputCorrect();
    }

    public void OpenLabelMenu(Edge edge, GameObject previousMenu)
    {
      _edge = edge;
      _previousMenu = previousMenu;
      labelMenu.SetActive(true);
      labelInput.text = _edge.Label;
    }

    public void SaveDataButtonOnClick()
    {
      if (!IsInputCorrect())
        return;

      _edge.SetLabel(labelInput.text);
      labelMenu.SetActive(false);
      _previousMenu.SetActive(false);
      GameService.Instance.UnPauseGameWithoutResume();
    }

    public void ExitButtonOnClick()
    {
      labelMenu.SetActive(false);
    }
  }
}
