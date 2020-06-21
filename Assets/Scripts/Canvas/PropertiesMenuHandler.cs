using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Canvas.PropertyContainer;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas
{
  public class PropertiesMenuHandler : MonoBehaviour
  {
    const string c_noneClassLabel = "None";

    APointContainer _aPointContainerObject;
    GraphService _graphService;

    Node _node;
    List<NodeClass> _nodeClasses;

    PointContainer _pointContainerObject;

    GameObject _previousMenu;

    VPointContainer _vPointContainerObject;

    public GameObject aPointContainer;

    public InputField labelInput;

    public Dropdown nodeClassDropdown;

    public GameObject pointContainer;

    public GameObject propertiesMenu;

    public Button saveButton;

    public GameObject vPointContainer;

    bool IsInputCorrect() =>
      _pointContainerObject.IsInputCorrect() &&
      _vPointContainerObject.IsInputCorrect() &&
      _aPointContainerObject.IsInputCorrect() &&
      labelInput.text.Length > 0;

    public void Start()
    {
      _pointContainerObject = pointContainer.GetComponent<PointContainer>();
      _vPointContainerObject = vPointContainer.GetComponent<VPointContainer>();
      _aPointContainerObject = aPointContainer.GetComponent<APointContainer>();

      labelInput.onValueChanged.AddListener(text => new LabelValidator(labelInput).OnValueChanged());

      propertiesMenu.SetActive(false);
    }

    public void Update()
    {
      if (_nodeClasses == null)
      {
        _graphService = FindObjectOfType<GraphService>();
        if (_graphService != null)
        {
          var classes = _graphService.Graph.NodeClasses;
          _nodeClasses = new List<NodeClass> {null}.Union(classes).ToList();
          nodeClassDropdown.ClearOptions();
          nodeClassDropdown.AddOptions(_nodeClasses.Select(n => n?.Name ?? c_noneClassLabel).ToList());
        }
      }

      saveButton.interactable = IsInputCorrect();
    }

    public void OpenPropertiesMenu(Node node, GameObject previousMenu)
    {
      _node = node;
      _previousMenu = previousMenu;
      _previousMenu.SetActive(false);
      propertiesMenu.SetActive(true);

      labelInput.text = _node.Label;
      nodeClassDropdown.value = _nodeClasses.IndexOf(_node.nodeClass);
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
      _graphService.SetNodeClass(_node, _nodeClasses[nodeClassDropdown.value]);
      GameService.Instance.UnPauseGameWithoutResume();
    }

    public void ExitButtonOnClick()
    {
      _previousMenu.SetActive(true);
      propertiesMenu.SetActive(false);
    }
  }
}
