using Assets.Scripts.Common;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class NodeGameObjectFactory : MonoBehaviour
  {
    TextureCache _materialCache;

    public GameObject nodePrefab;
    public GameObject ghostNodePrefab;
    public GameObject nodeCanvasPrefab;

    const PrimitiveType c_defaultNodeType = PrimitiveType.Sphere;

    public void Start()
    {
      _materialCache = FindObjectOfType<TextureCache>();
    }


    public GameObject CreateNodeGameObject(Node node)
    {
      if (_materialCache == null)
        _materialCache = FindObjectOfType<TextureCache>();

      if (node.nodeClass == null)
        return CreateDefaultNode(node.Position, Quaternion.Euler(node.Rotation));

      var material = _materialCache.GetByClassId(node.nodeClass.id);

      var gameObject = node.nodeClass.objectType == null
        ? GameObject.CreatePrimitive(c_defaultNodeType)
        : GameObject.CreatePrimitive(node.nodeClass.objectType.Value);

      gameObject.transform.localScale = Vector3.one * (node.nodeClass.scale ?? 1);
      gameObject.transform.position = node.Position;
      gameObject.transform.rotation = Quaternion.Euler(node.Rotation);
      gameObject.tag = Constants.PhysicalNodeTag;

      var renderer = gameObject.GetComponent<Renderer>();
      renderer.material = material;

      Instantiate(nodeCanvasPrefab, gameObject.transform);

      return gameObject;
    }


    public GameObject CreateGhostNodeGameObject(Vector3 position, Quaternion rotation) =>
      Instantiate(ghostNodePrefab, position, rotation);

    public GameObject CreateDefaultNode(Vector3 position, Quaternion rotation) =>
      Instantiate(nodePrefab, position, rotation);

    void Log(string message)
    {
      Debug.Log(message);
    }
  }
}
