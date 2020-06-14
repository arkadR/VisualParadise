using Assets.Scripts.Canvas.PropertyContainer;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas
{
  public class PropertiesMenuHandler : MonoBehaviour
  {
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

    private bool AreValuesCorrect()
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
      saveButton.interactable = AreValuesCorrect();
    }

    public void OpenPropertiesMenu(Node node)
    {
      _node = node;
      propertiesMenu.SetActive(true);

      labelInput.text = _node.label;
      _pointContainerObject.SetNode(_node);
      _vPointContainerObject.SetNode(_node);
      _aPointContainerObject.SetNode(_node);
    }

    public void SaveDataButtonOnClick()
    {
      if (!AreValuesCorrect())
        return;

      _node.UpdateLabel(labelInput.text);
      _pointContainerObject.SaveData();
      _vPointContainerObject.SaveData();
      _aPointContainerObject.SaveData();
      ExitButtonOnClick();
    }

    public void ExitButtonOnClick()
    {
      propertiesMenu.SetActive(false);
    }
  }
}
