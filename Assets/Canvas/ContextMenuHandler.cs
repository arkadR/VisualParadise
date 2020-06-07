using System.Collections.Generic;
using System.Linq;
using Assets.GameObject;
using UnityEngine;

public class ContextMenuHandler : MonoBehaviour
{
    private PhysicalNode PhysicalNode;

    private List<PhysicalEdge> PhysicalEdges;

    private List<PhysicalNode> PhysicalNodes;

    public GameObject ContextMenu;

    public void OpenContextMenu(List<PhysicalNode> physicalNodes, List<PhysicalEdge> physicalEdges, GameObject node)
    {
        var physicalNode = physicalNodes.SingleOrDefault(x => GameObject.ReferenceEquals(x.physicalNode, node));
        PhysicalNode = physicalNode;
        PhysicalNodes = physicalNodes;
        PhysicalEdges = physicalEdges;
        ContextMenu.SetActive(true);
        GameService.Instance.PauseGame();
    }

    public void ChangeParametersButtonOnClick()
    {
        FindObjectOfType<PropertiesMenuHandler>().SetPhysicalNode(PhysicalNode);
    }

    public void DeleteButtonOnClick()
    {
        PhysicalNodes.Remove(PhysicalNode);
        Destroy(PhysicalNode.physicalNode);
        var physicalEdgesToDelete = PhysicalEdges.FindAll(x => x.nodeFrom.Equals(PhysicalNode) || x.nodeTo.Equals(PhysicalNode));
        PhysicalEdges.RemoveAll(x => physicalEdgesToDelete.Contains(x));
        foreach (var physicalEdge in physicalEdgesToDelete)
        {
            Destroy(physicalEdge.edge);
        }
        ExitButtonOnClick();
    }

    public void ExitButtonOnClick()
    {
        ContextMenu.SetActive(false);
        GameService.Instance.UnPauseGame();
    }
}
