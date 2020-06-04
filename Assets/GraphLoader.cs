using System;
using System.Collections.Generic;
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
            var sphere = NodeGenerator.GeneratePhysicalNode(node.point.Position(),
                node.point.Rotation(),
                nodeMaterial);
            physicalNodes.Add(new PhysicalNode { id = node.id, node = node, physicalNode = sphere });
        }

        var edgeMaterial = Resources.Load<Material>("Materials/Edge Material");
        foreach (var edge in graph.edges)
        {
            var node1 = graph.nodes.Single(n => n.id == edge.from);
            var node2 = graph.nodes.Single(n => n.id == edge.to);
            var line = EdgeGenerator.CreateGameObjectEdge(node1, node2, edgeMaterial);
            physicalEdges.Add(new PhysicalEdge
            {
                edge = line,
                nodeFrom = physicalNodes.Single(pn => pn.id == edge.from),
                nodeTo = physicalNodes.Single(pn => pn.id == edge.to)
            });
        }
    }

    Graph LoadGraph(string fileName)
    {
        var graphJson = Resources.Load(fileName).ToString();
        return JsonUtility.FromJson<Graph>(graphJson);
    }

    public PhysicalNode FindPhysicalNodeByGameObject(GameObject gameObjectParam)
    {
        return physicalNodes
            .First(pn => pn.physicalNode == gameObjectParam);
    }

    public PhysicalEdge FindPhysicalEdgeByPhysicalNodes(PhysicalNode first, PhysicalNode second)
    {
        var nodes = new HashSet<PhysicalNode> { first, second };
        return physicalEdges
            .FirstOrDefault(
                edge => nodes.Contains(edge.nodeFrom)
                        && nodes.Contains(edge.nodeTo)
            );
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

        public static Node ZeroNode(int id) => new Node { id = id, point = new Point(), apoint = new APoint(), vpoint = new VPoint() };
    }

    [Serializable()]
    public class Point
    {
        public float x, y, z, theta, phi, psi;

        public Vector3 Position() => new Vector3(x, y, z);
        public Quaternion Rotation() => Quaternion.Euler(theta, phi, psi);
        public void SetPosition(Vector3 position)
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }
    }

    [Serializable]
    public class VPoint
    {
        public float vx, vy, vz, om_theta, om_phi, om_psi;

        public Vector3 Velocity() => new Vector3(vx, vy, vz);
        public Vector3 AngVelocity() => new Vector3(om_theta, om_phi, om_psi);

        public void SetVelocity(Vector3 velocity)
        {
            vx = velocity.x;
            vy = velocity.y;
            vz = velocity.z;
        }

        public void SetAngVelocity(Vector3 angVelocity)
        {
            om_theta = angVelocity.x;
            om_phi = angVelocity.y;
            om_psi = angVelocity.z;
        }
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