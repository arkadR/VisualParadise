using UnityEngine;

namespace Assets.Scripts
{
  public static class NodeGenerator
  {
    public static GameObject GeneratePhysicalNode(Vector3 position, Quaternion rotation, Material nodeMaterial)
    {
      var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      sphere.transform.position = position;
      sphere.transform.rotation = rotation;
      sphere.GetComponent<Renderer>().material = nodeMaterial;
      sphere.tag = Constants.PhysicalNodeTag;
      return sphere;
    }
  }
}
