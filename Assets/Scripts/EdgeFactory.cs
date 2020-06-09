using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class EdgeFactory : MonoBehaviour
  {
    [SerializeField] private GameObject edgePrefab;

    public GameObject CreateEdge(Node fromNode, Node toNode)
    {
      var line = Instantiate(edgePrefab);
      var lineRenderer = line.GetComponent<LineRenderer>();
      lineRenderer.SetPosition(0, fromNode.Position);
      lineRenderer.SetPosition(1, toNode.Position);
      return line;
    }
  }
}
