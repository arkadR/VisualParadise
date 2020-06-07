using UnityEngine;

public static class NodeGenerator
{
    public static GameObject GeneratePhysicalNode(Vector3 position, Quaternion rotation, Material nodeMaterial)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.rotation = rotation;
        sphere.GetComponent<Renderer>().material = nodeMaterial;
        sphere.tag = Constants.PhysicalNodeTag;
        AddNodeRigidBody(sphere);
        AddCollider(sphere);
        return sphere;
    }

    private static void AddCollider(GameObject node)
    {
        var collider = node.AddComponent<SphereCollider>();
        collider.enabled = true;
    }

    private static void AddNodeRigidBody(GameObject node)
    {
        Rigidbody sphereRigidBody = node.AddComponent<Rigidbody>();
        sphereRigidBody.maxAngularVelocity = 20;
        sphereRigidBody.useGravity = false;
    }
}