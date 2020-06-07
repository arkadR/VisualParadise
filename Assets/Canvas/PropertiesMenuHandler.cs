using Assets.Canvas.PropertyContainer;
using UnityEngine;
using static GraphLoader;

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

    public void OnSave()
    {
        pointContainerObject.SaveData();
        vPointContainerObject.SaveData();
        aPointContainerObject.SaveData();
        OnClose();
    }

    public void OnClose()
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
