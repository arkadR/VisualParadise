using Assets.Canvas.PropertyContainer;
using Assets.GameObject;
using UnityEngine;

public class PropertiesMenuHandler : MonoBehaviour
{
  private PointContainer pointContainerObject;

  private VPointContainer vPointContainerObject;

  private APointContainer aPointContainerObject;

  private PhysicalNode PhysicalNode;

  public GameObject PropertiesMenu;

  public GameObject PointContainer;

  public GameObject VPointContainer;

  public GameObject APointContainer;

  public void SetPhysicalNode(PhysicalNode physicalNode)
  {
    PhysicalNode = physicalNode;
    PropertiesMenu.SetActive(true);

    pointContainerObject.SetPhysicalNode(physicalNode);
    vPointContainerObject.SetPhysicalNode(physicalNode);
    aPointContainerObject.SetPhysicalNode(physicalNode);
  }

  public void SaveDataButtonOnClick()
  {
    pointContainerObject.SaveData();
    vPointContainerObject.SaveData();
    aPointContainerObject.SaveData();
    ExitButtonOnClick();
  }

  public void ExitButtonOnClick()
  {
    PropertiesMenu.SetActive(false);
  }

  public void Start()
  {
    pointContainerObject = PointContainer.GetComponent<PointContainer>();
    vPointContainerObject = VPointContainer.GetComponent<VPointContainer>();
    aPointContainerObject = APointContainer.GetComponent<APointContainer>();
  }
}