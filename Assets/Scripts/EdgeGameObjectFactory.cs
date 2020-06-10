using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class EdgeGameObjectFactory : MonoBehaviour
  {
    public GameObject edgePrefab;

    public GameObject CreateEdgeGameObject(Node fromNode, Node toNode)
    {
      var line = Instantiate(edgePrefab);
      var lineRenderer = line.GetComponent<LineRenderer>();
      lineRenderer.SetPosition(0, fromNode.Position);
      lineRenderer.SetPosition(1, toNode.Position);
      return line;
    }
  }
}
