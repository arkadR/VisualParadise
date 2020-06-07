using Player.Guns;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GraphLoader;

public class ContextMenuHandler : MonoBehaviour
{
    private PhysicalNode PhysicalNode;

    private List<PhysicalEdge> PhysicalEdges;

    private List<PhysicalNode> PhysicalNodes;

    public GameObject ContextMenu;

    public void SetPhysicalNode(List<PhysicalNode> physicalNodes, List<PhysicalEdge> physicalEdges, GameObject node)
    {
        var physicalNode = physicalNodes.SingleOrDefault(x => GameObject.ReferenceEquals(x.physicalNode, node));
        PhysicalNode = physicalNode;
        PhysicalNodes = physicalNodes;
        PhysicalEdges = physicalEdges;
        ContextMenu.SetActive(true);
        MouseLook.SetFreeLookDisabled(true);
        GunController.InputDisabled = true;
    }

    public void OnEditProperties()
    {
        FindObjectOfType<PropertiesMenuHandler>().SetPhysicalNode(PhysicalNode);
    }

    public void OnDelete()
    {
        PhysicalNodes.Remove(PhysicalNode);
        Destroy(PhysicalNode.physicalNode);
        var physicalEdgesToDelete = PhysicalEdges.FindAll(x => x.nodeFrom.Equals(PhysicalNode) || x.nodeTo.Equals(PhysicalNode));
        PhysicalEdges.RemoveAll(x => physicalEdgesToDelete.Contains(x));
        foreach (var physicalEdge in physicalEdgesToDelete)
        {
            Destroy(physicalEdge.edge);
        }
        OnCloseContextMenu();
    }

    public void OnCloseContextMenu()
    {
        ContextMenu.SetActive(false);
        MouseLook.SetFreeLookDisabled(false);
        GunController.InputDisabled = false;
    }
}
