using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class GraphService : MonoBehaviour
  {
    public EdgeGameObjectFactory edgeGameObjectFactory;
    public NodeGameObjectFactory nodeGameObjectFactory;
    public Graph Graph { get; private set; }
    private bool _labelVisibility = false;
    public bool LabelVisibility
    {
      get => _labelVisibility;
      set
      {
        _labelVisibility = value;
        Graph?.nodes.ForEach(n => n.Text.enabled = _labelVisibility);
        Graph?.edges.ForEach(n => n.Text.enabled = _labelVisibility);
      }
    }

    public void ToggleLabelVisibility() => LabelVisibility = !LabelVisibility;

    public void SetGraph(Graph graph)
    {
      Graph = graph;

      foreach (var node in graph.nodes)
      {
        var sphere = nodeGameObjectFactory.CreateNodeGameObject(node.Position,
          Quaternion.Euler(node.Rotation));
        node.gameObject = sphere;
        node.gameObject.GetComponentInChildren<Text>().text = node.label;
      }

      foreach (var edge in graph.edges)
      {
        var node1 = graph.nodes.Single(n => n.id == edge.from);
        var node2 = graph.nodes.Single(n => n.id == edge.to);
        var line = edgeGameObjectFactory.CreateEdgeGameObject(node1, node2);
        edge.gameObject = line;
        edge.gameObject.GetComponentInChildren<Text>().text = edge.label;
      }

      LabelVisibility = false;
    }

    public bool IsNode(GameObject gameObject) => FindNodeByGameObject(gameObject) != null;

    public Node FindNodeByGameObject(GameObject gameObject) =>
      Graph.nodes.SingleOrDefault(n => n.gameObject == gameObject);

    public List<Edge> FindNodeEdges(Node node) => Graph.edges.Where(e => e.from == node.id || e.to == node.id).ToList();

    public Edge FindEdgeByNodes(Node node1, Node node2) =>
      Graph.edges.SingleOrDefault(e =>
        (e.from == node1.id && e.to == node2.id) ||
        (e.from == node2.id && e.to == node1.id));

    public void AddNode(Vector3 position, Quaternion rotation)
    {
      var id = Graph.nodes.Any()
        ? Graph.nodes.Max(n => n.id) + 1
        : 0;

      var node = Node.EmptyNode(id, nodeGameObjectFactory.CreateNodeGameObject(position, rotation));
      node.Position = position;
      node.Rotation = rotation.eulerAngles;
      node.Text.enabled = LabelVisibility;
      Graph.nodes.Add(node);
    }

    public void AddEdge(Node node1, Node node2)
    {
      var edge = new Edge
      {
        from = node1.id,
        to = node2.id,
        gameObject = edgeGameObjectFactory.CreateEdgeGameObject(node1, node2),
        nodeFrom = node1,
        nodeTo = node2
      };
      edge.label = edge.DefaultLabel;
      edge.Text.text = edge.label;
      edge.Text.enabled = LabelVisibility;
      Graph.edges.Add(edge);
    }

    public void RemoveNode(Node node)
    {
      Destroy(node.gameObject);
      Graph.nodes.Remove(node);
      var edgesToRemove = Graph.edges.Where(e => e.from == node.id || e.to == node.id).ToList();

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

    public Node FindNodeById(int id) => Graph.nodes.SingleOrDefault(n => n.id == id);

    public void FixEdge(Edge edge)
    {
      var lineRenderer = edge.gameObject.GetComponent<LineRenderer>();
      var startingNode = FindNodeById(edge.from);
      var endingNode = FindNodeById(edge.to);
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
  }
}
