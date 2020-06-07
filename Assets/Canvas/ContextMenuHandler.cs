using System.Collections.Generic;
using System.Linq;
using Assets.Model;
using UnityEngine;

public class ContextMenuHandler : MonoBehaviour
{
    private Node _node;

    private List<Edge> _edges;

    private List<Node> _nodes;

    public GameObject contextMenu;

    public void OpenContextMenu(List<Node> nodes, List<Edge> edges, GameObject gameObjectHit)
    {
        var node = nodes.SingleOrDefault(x => ReferenceEquals(x.gameObject, gameObjectHit));
        _node = node;
        _nodes = nodes;
        _edges = edges;
        contextMenu.SetActive(true);
        GameService.Instance.PauseGame();
    }

    public void ChangeParametersButtonOnClick()
    {
        FindObjectOfType<PropertiesMenuHandler>().OpenPropertiesMenu(_node);
    }

    public void DeleteButtonOnClick()
    {
        _nodes.Remove(_node);
        Destroy(_node.gameObject);
        var physicalEdgesToDelete = _edges.FindAll(x => x.nodeTo.Equals(_node) || x.nodeTo.Equals(_node));
        _edges.RemoveAll(x => physicalEdgesToDelete.Contains(x));
        foreach (var physicalEdge in physicalEdgesToDelete)
        {
            Destroy(physicalEdge.gameObject);
        }
        ExitButtonOnClick();
    }

    public void ExitButtonOnClick()
    {
        contextMenu.SetActive(false);
        GameService.Instance.UnPauseGame();
    }
}
