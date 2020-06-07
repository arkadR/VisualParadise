using System.Collections.Generic;
using System.Linq;
using Assets.GameObject;
using Assets.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class GraphService : MonoBehaviour
  {
    public Graph Graph { get; private set; }

    public List<PhysicalNode> physicalNodes = new List<PhysicalNode>();
    public List<PhysicalEdge> physicalEdges = new List<PhysicalEdge>();


    public void SetGraph(Graph graph)
    {
      Graph = graph;

      var nodeMaterial = Resources.Load<Material>("Materials/Node Material");

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


    public PhysicalNode FindPhysicalNodeByGameObject(UnityEngine.GameObject gameObjectParam)
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

    public void AddNode(Vector3 position, Quaternion rotation, Material nodeMaterial)
    {
      var sphere = NodeGenerator.GeneratePhysicalNode(
        position,
        rotation,
        nodeMaterial);

      var id = physicalNodes.Count;
      var node = Node.ZeroNode(id);
      node.point.SetPosition(position);
      Graph.nodes.Add(node);
      physicalNodes.Add(
        new PhysicalNode
        {
          id = id,
          node = node,
          physicalNode = sphere
        }
      );
    }

    public void AddEdge(Node node1, Node node2, Material material)
    {
      var edgeObject = EdgeGenerator.CreateGameObjectEdge(
        node1,
        node2,
        material
      );

      var pn1 = physicalNodes.Single(n => n.node == node1);
      var pn2 = physicalNodes.Single(n => n.node == node2);
      var physicalEdge = new PhysicalEdge
      {
        edge = edgeObject,
        nodeFrom = pn1,
        nodeTo = pn2
      };
      physicalEdges.Add(physicalEdge);
    }
  }
}