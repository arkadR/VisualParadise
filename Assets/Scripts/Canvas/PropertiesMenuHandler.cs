using Assets.Scripts.Canvas.PropertyContainer;
using Assets.Scripts.Model;
using UnityEngine;

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
}
