using Assets.Scripts.Canvas.PropertyContainer;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas
{
  public class PropertiesMenuHandler : MonoBehaviour
  {
    private UnityEngine.GameObject _previousMenu;

    private PointContainer _pointContainerObject;

    private VPointContainer _vPointContainerObject;

    private APointContainer _aPointContainerObject;

    private Node _node;

    public UnityEngine.GameObject propertiesMenu;

    public UnityEngine.GameObject pointContainer;

    public UnityEngine.GameObject vPointContainer;

    public UnityEngine.GameObject aPointContainer;

    public InputField labelInput;

    public Button saveButton;

    private bool IsInputCorrect()
    {
      return (_pointContainerObject.IsInputCorrect() && _vPointContainerObject.IsInputCorrect()
        && _aPointContainerObject.IsInputCorrect() && labelInput.text.Length > 0);
    }

    public void Start()
    {
      _pointContainerObject = pointContainer.GetComponent<PointContainer>();
      _vPointContainerObject = vPointContainer.GetComponent<VPointContainer>();
      _aPointContainerObject = aPointContainer.GetComponent<APointContainer>();

      labelInput.onValueChanged.AddListener((text) => new LabelValidator(labelInput).OnValueChanged());

      propertiesMenu.SetActive(false);
    }

    public void Update()
    {
      saveButton.interactable = IsInputCorrect();
    }

    public void OpenPropertiesMenu(Node node, GameObject previousMenu)
    {
      _node = node;
      _previousMenu = previousMenu;
      propertiesMenu.SetActive(true);

      labelInput.text = _node.label;
      _pointContainerObject.SetNode(_node);
      _vPointContainerObject.SetNode(_node);
      _aPointContainerObject.SetNode(_node);
    }

    public void SaveDataButtonOnClick()
    {
      if (!IsInputCorrect())
        return;

      _node.UpdateLabel(labelInput.text);
      _pointContainerObject.SaveData();
      _vPointContainerObject.SaveData();
      _aPointContainerObject.SaveData();
      propertiesMenu.SetActive(false);
      _previousMenu.SetActive(false);
      GameService.Instance.UnPauseGameWithoutResume();
    }

    public void ExitButtonOnClick()
    {
      propertiesMenu.SetActive(false);
    }
  }
}
