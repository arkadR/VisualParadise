using Assets.Canvas.PropertyContainer;
using Assets.Model;
using UnityEngine;

public class PropertiesMenuHandler : MonoBehaviour
{
  private PointContainer _pointContainerObject;

  private VPointContainer _vPointContainerObject;

  private APointContainer _aPointContainerObject;

  private Node _node;

  public GameObject propertiesMenu;

  public GameObject pointContainer;

  public GameObject vPointContainer;

  public GameObject aPointContainer;

  public void OpenPropertiesMenu(Node node)
  {
    _node = node;
    propertiesMenu.SetActive(true);

    _pointContainerObject.SetNode(_node);
    _vPointContainerObject.SetNode(_node);
    _aPointContainerObject.SetNode(_node);
  }

  public void SaveDataButtonOnClick()
  {
    _pointContainerObject.SaveData();
    _vPointContainerObject.SaveData();
    _aPointContainerObject.SaveData();
    ExitButtonOnClick();
  }

  public void ExitButtonOnClick()
  {
    propertiesMenu.SetActive(false);
  }

  public void Start()
  {
    _pointContainerObject = pointContainer.GetComponent<PointContainer>();
    _vPointContainerObject = vPointContainer.GetComponent<VPointContainer>();
    _aPointContainerObject = aPointContainer.GetComponent<APointContainer>();
  }
}