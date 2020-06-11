using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class GraphService : MonoBehaviour
  {
    public EdgeGameObjectFactory edgeGameObjectFactory;
    public NodeGameObjectFactory nodeGameObjectFactory;
    public Graph Graph { get; private set; }

    public void SetGraph(Graph graph)
    {
      Graph = graph;

      foreach (var node in graph.nodes)
      {
        var sphere = nodeGameObjectFactory.CreateNodeGameObject(node.Position,
          Quaternion.Euler(node.Rotation));
        node.gameObject = sphere;
      }

      foreach (var edge in graph.edges)
      {
        //TODO[ME] EXTREMELY POORLY PERFORMING
        edgeGameObjectFactory.CreateGameObjectEdgesFor(new HashSet<Edge> {edge});
      }
    }

    public bool IsNode(GameObject gameObject) => FindNodeByGameObject(gameObject) != null;

    public Node FindNodeByGameObject(GameObject gameObject) =>
      Graph.nodes.SingleOrDefault(n => n.gameObject == gameObject);

    public List<Edge> FindNodeEdges(Node node) => Graph.edges.Where(e => e.from == node.id || e.to == node.id).ToList();

    public ISet<Edge> FindEdgesByNodes(Node node1, Node node2)
    {
      var nodes = new HashSet<Node> {node1, node2};
      return (from edge in Graph.edges
          where nodes.Contains(edge.nodeFrom) && nodes.Contains(edge.nodeTo)
          select edge)
        .ToSet();
    }

    public void AddNode(Vector3 position, Quaternion rotation)
    {
      var id = Graph.nodes.Any()
        ? Graph.nodes.Max(n => n.id) + 1
        : 0;

      var node = Node.EmptyNode(id, nodeGameObjectFactory.CreateNodeGameObject(position, rotation));
      node.Position = position;
      node.Rotation = rotation.eulerAngles;
      Graph.nodes.Add(node);
    }

    public void AddEdge(Node node1, Node node2)
    {
      var id = Graph.edges.Any()
        ? Graph.edges.Max(n => n.id) + 1
        : 0;
      var edge = new Edge
      {
        id = id,
        from = node1.id,
        to = node2.id,
        nodeFrom = node1,
        nodeTo = node2
      };
      var existingEdges = FindEdgesByNodes(node1, node2);
      existingEdges.Add(edge);
      edgeGameObjectFactory.CreateGameObjectEdgesFor(existingEdges);
      Graph.edges.Add(edge);
    }

    public void RemoveNode(Node node)
    {
      Destroy(node.gameObject);
      Graph.nodes.Remove(node);
      var edgesToRemove = Graph.edges.Where(e => e.from == node.id || e.to == node.id).ToList();

      foreach (var edge in edgesToRemove)
      {
        RemoveAllEdgesBetween(edge.nodeFrom, edge.nodeTo);
      }
    }

    public void RemoveAllEdgesBetween(Node node1, Node node2)
    {
      var edges = FindEdgesByNodes(node1, node2);
      foreach (var edge in edges)
      {
        Destroy(edge.gameObject);
        Graph.edges.Remove(edge);
      }
    }

    public void FixEdge(Edge edge)
    {
      var lineRenderer = edge.gameObject.GetComponent<LineRenderer>();
      var startingNode = Graph.FindNodeById(edge.from);
      var endingNode = Graph.FindNodeById(edge.to);
      lineRenderer.SetPosition(0, startingNode.Position);
      lineRenderer.SetPosition(1, endingNode.Position);
    }

    /// <summary>
    ///   Update edges positions based on corresponding nodes
    /// </summary>
    public void FixEdges()
    {
      foreach (var e in Graph.edges)
      {
        FixEdge(e);
      }
    }

    /// <summary>
    ///   Set velocity of all nodes to 0
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
