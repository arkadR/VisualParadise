using UnityEngine;

public static class NodeGenerator
{
    public static GameObject GenerateNode(Vector3 position, Quaternion rotation, Material nodeMaterial)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.rotation = rotation;
        sphere.GetComponent<Renderer>().material = nodeMaterial;
        sphere.tag = Tags.PhysicalNodeTag;
        AddNodeRigidbody(sphere);
        AddColider(sphere);
        return sphere;
    }

    private static void AddColider(GameObject node)
    {
        var collider = node.AddComponent<SphereCollider>();
        collider.enabled = true;
    }

    private static void AddNodeRigidbody(GameObject node)
    {
        Rigidbody sphereRigidBody = node.AddComponent<Rigidbody>();
        sphereRigidBody.maxAngularVelocity = 20;
        sphereRigidBody.useGravity = false;
    }
}