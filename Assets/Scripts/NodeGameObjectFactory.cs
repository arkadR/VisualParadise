using UnityEngine;

namespace Assets.Scripts
{
  public class NodeGameObjectFactory : MonoBehaviour
  {
    public GameObject nodePrefab;
    public GameObject ghostNodePrefab;

    public GameObject CreateNodeGameObject(Vector3 position, Quaternion rotation) =>
      Instantiate(nodePrefab, position, rotation);

    public GameObject CreateGhostNodeGameObject(Vector3 position, Quaternion rotation) =>
      Instantiate(ghostNodePrefab, position, rotation);
  }
}
