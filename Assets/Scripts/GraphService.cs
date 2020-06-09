using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Model;
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
       
    public List<Edge> FindNodeEdges(Node node) => Graph.edges.FindAll(e => e.from == node.id || e.to == node.id);

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
        nodeFrom = node1,
        nodeTo = node2
      };
      Graph.edges.Add(edge);
    }

    public void RemoveNode(Node node)
    {
      Destroy(node.gameObject);
      Graph.nodes.Remove(node);
      var edgesToRemove = Graph.edges.FindAll(e => e.from == node.id || e.to == node.id);
      foreach (var edge in edgesToRemove)
      {
        Destroy(edge.gameObject);
        Graph.edges.Remove(edge);
      }
    }

    public Node FindNodeById(int id)
    {
      return Graph.nodes.SingleOrDefault(n => n.id == id);
    }

    public void FixEdge(Edge edge)
    {
      var lineRenderer = edge.gameObject.GetComponent<LineRenderer>();
      var startingNode = FindNodeById(edge.from);
      var endingNode = FindNodeById(edge.to);
      lineRenderer.SetPosition(0, startingNode.Position);
      lineRenderer.SetPosition(1, endingNode.Position);
    }
  }
}
