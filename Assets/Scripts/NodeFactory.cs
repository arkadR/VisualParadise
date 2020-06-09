using UnityEngine;

namespace Assets.Scripts
{
  public class NodeFactory : MonoBehaviour
  {
    public GameObject nodePrefab;

    public GameObject CreateNode(Vector3 position, Quaternion rotation)
    {
      return Instantiate(nodePrefab, position, rotation);
    }
  }
}
