using System.Linq;
using Assets.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class GraphService : MonoBehaviour
  {
    public Graph Graph { get; private set; }

    public void SetGraph(Graph graph)
    {
      Graph = graph;

      var nodeMaterial = Resources.Load<Material>("Materials/Node Material");
      var edgeMaterial = Resources.Load<Material>("Materials/Edge Material");

      foreach (var node in graph.nodes)
      {
        var sphere = NodeGenerator.GeneratePhysicalNode(node.Position,
          Quaternion.Euler(node.Rotation),
          nodeMaterial);
        node.gameObject = sphere;
      }

      foreach (var edge in graph.edges)
      {
        var node1 = graph.nodes.Single(n => n.id == edge.from);
        var node2 = graph.nodes.Single(n => n.id == edge.to);
        var line = EdgeGenerator.CreateGameObjectEdge(node1, node2, edgeMaterial);
        edge.gameObject = line;
      }
    }


    public Node FindNodeByGameObject(UnityEngine.GameObject gameObject) => Graph.nodes.SingleOrDefault(n => n.gameObject == gameObject);

    public Edge FindEdgeByNodes(Node node1, Node node2) => Graph.edges.SingleOrDefault(e => e.@from == node1.id && e.to == node2.id);

    public void AddNode(Vector3 position, Quaternion rotation, Material nodeMaterial)
    {
      var id = Graph.nodes.Count;
      var node = Node.EmptyNode(id, NodeGenerator.GeneratePhysicalNode(position, rotation, nodeMaterial));
      node.Position = position;
      node.Rotation = rotation.eulerAngles;
      Graph.nodes.Add(node);
    }

    public void AddEdge(Node node1, Node node2, Material material)
    {
      var edge = new Edge
      {
        from = node1.id,
        to = node2.id,
        gameObject = EdgeGenerator.CreateGameObjectEdge(node1, node2, material),
      };
      Graph.edges.Add(edge);
    }
  }
}