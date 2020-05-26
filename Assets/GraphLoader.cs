using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GraphLoader : MonoBehaviour
{
    public List<PhysicalNode> physicalNodes = new List<PhysicalNode>();
    public List<PhysicalEdge> physicalEdges = new List<PhysicalEdge>();

    // Start is called before the first frame update
    void Start()
    {
        var nodeMaterial = Resources.Load<Material>("Materials/Node Material");
        var graph = LoadGraph("graph2_small");
        foreach (var node in graph.nodes)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = node.point.Position();
            sphere.transform.rotation = node.point.Rotation();
            sphere.GetComponent<Renderer>().material = nodeMaterial;
            AddNodeRigidbody(sphere);
            physicalNodes.Add(new PhysicalNode { id = node.id, node = node, physicalNode = sphere });
        }

        var edgeMaterial = Resources.Load<Material>("Materials/Edge Material");
        foreach (var edge in graph.edges)
        {
            var node1 = graph.nodes.Single(n => n.id == edge.from);
            var node2 = graph.nodes.Single(n => n.id == edge.to);
            var line = new GameObject();
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(0, node1.point.Position());
            lineRenderer.SetPosition(1, node2.point.Position());
            lineRenderer.startWidth = 0.2f;
            lineRenderer.material = edgeMaterial;
            physicalEdges.Add(new PhysicalEdge { edge = line, nodeFrom = physicalNodes.Single(pn => pn.id == edge.from), nodeTo = physicalNodes.Single(pn => pn.id == edge.to) });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddNodeRigidbody(GameObject node)
    {
        Rigidbody sphereRigidBody = node.AddComponent<Rigidbody>();
        sphereRigidBody.maxAngularVelocity = 20;
        sphereRigidBody.useGravity = false;
    }


    Graph LoadGraph(string fileName)
    {
        var graphJson = Resources.Load(fileName).ToString();
        return JsonUtility.FromJson<Graph>(graphJson);
    }

    [Serializable]
    class Graph
    {
        public List<Node> nodes;
        public List<Edge> edges;
    }

    [Serializable]
    public class Node
    {
        public int id;
        public Point point;
        public VPoint vpoint;
        public APoint apoint;
    }

    [Serializable()]
    public class Point
    {
        public float x, y, z, theta, phi, psi;

        public Vector3 Position() => new Vector3(x, y, z);
        public Quaternion Rotation() => Quaternion.Euler(theta, phi, psi);
    }

    [Serializable]
    public class VPoint
    {
        public float vx, vy, vz, om_theta, om_phi, om_psi;

        public Vector3 Velocity() => new Vector3(vx, vy, vz);
        public Vector3 AngVelocity() => new Vector3(om_theta, om_phi, om_psi);
    }

    [Serializable]
    public class APoint
    {
        public float ax, ay, az, alpha_theta, alpha_phi, alpha_psi;

        public Vector3 Acceleration() => new Vector3(ax, ay, az);
        public Vector3 AngAcceleration() => new Vector3(alpha_theta, alpha_phi, alpha_psi);
    }

    [Serializable]
    class Edge
    {
        public int from;
        public int to;
        public int weight;
    }

    public class PhysicalEdge
    {
        public PhysicalNode nodeFrom;
        public PhysicalNode nodeTo;
        public GameObject edge;
    }

    public class PhysicalNode
    {
        public int id;
        public Node node;
        public GameObject physicalNode;
    }
}
