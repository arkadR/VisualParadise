using System.Linq;
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

    public Edge FindEdgeByNodes(Node node1, Node node2) {
      var edge = Graph.edges.SingleOrDefault(e => e.@from == node1.id && e.to == node2.id);
      if (edge == null)
        return Graph.edges.SingleOrDefault(e => e.@from == node2.id && e.to == node1.id);
      return edge;
    }

    public void AddNode(Vector3 position, Quaternion rotation, Material nodeMaterial)
    {
      var id = Graph.nodes.Max(n => n.id) + 1;
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
      var edgesToRemove = Graph.edges.Where(e => e.from == node.id || e.to == node.id);
      foreach (var edge in edgesToRemove)
      {
        RemoveEdge(edge);
      }
    }

    public void RemoveEdge(Edge edge)
    {
      Destroy(edge.gameObject);
      Graph.edges.Remove(edge);
    }

    public Node FindNodeById(int id)
    {
      return Graph.nodes.SingleOrDefault(n => n.id == id);
    }

    /// <summary>
    /// Update edges position based on corresponding nodes
    /// </summary>
    public void FixEdges()
    {
      foreach (var e in Graph.edges)
      {
        var lineRenderer = e.gameObject.GetComponent<LineRenderer>();
        var startingNode = FindNodeById(e.from);
        var endingNode = FindNodeById(e.to);
        lineRenderer.SetPosition(0, startingNode.Position);
        lineRenderer.SetPosition(1, endingNode.Position);
      }
    }

    /// <summary>
    /// Set velocity of all nodes to 0
    /// </summary>
    public void StopNodes()
    {
      foreach (var n in Graph.nodes)
      {
        n.Velocity = Vector3.zero;
        n.AngularVelocity = Vector3.zero;
        n.Acceleration = Vector3.zero;
        n.AngularAcceleration = Vector3.zero;
      }
    }
  }
}
