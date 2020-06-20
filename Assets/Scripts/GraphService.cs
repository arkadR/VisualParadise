using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Edges;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class GraphService : MonoBehaviour
  {
    bool _labelVisibility;
    public EdgeGameObjectFactory edgeGameObjectFactory;
    public LineFactory lineFactory;
    public NodeGameObjectFactory nodeGameObjectFactory;
    public Graph Graph { get; private set; }

    public bool LabelVisibility
    {
      get => _labelVisibility;
      set
      {
        _labelVisibility = value;
        Graph?.Nodes.ForEach(n => n.Text.enabled = _labelVisibility);
        Graph?.Edges.ForEach(n => n.Text.enabled = _labelVisibility);
      }
    }

    public void ToggleLabelVisibility() => LabelVisibility = !LabelVisibility;

    public void SetGraph(Graph graph)
    {
      Graph = graph;

      foreach (var node in graph.Nodes)
      {
        var sphere = nodeGameObjectFactory.CreateNodeGameObject(node);
        node.gameObject = sphere;
        node.gameObject.GetComponentInChildren<Text>().text = node.Label;
      }

      foreach (var edge in Graph.Edges)
      {
        edge.nodeFrom = FindNodeById(edge.From);
        edge.nodeTo = FindNodeById(edge.To);
      }

      foreach (var edgeGroup in Graph.GetEdgesGroupedByNodes().Values)
      {
        var x = edgeGameObjectFactory.CreateGameObjectEdgesFor(edgeGroup, LabelVisibility);
        for (var i = 0; i < edgeGroup.Count; i++)
        {
          var (segmentGroup, label) = x[i];
          edgeGroup[i].segmentGroup = segmentGroup;
          edgeGroup[i].labelGameObject = label;
        }
      }

      LabelVisibility = false;
    }

    public bool IsNode(GameObject gameObject) => FindNodeByGameObject(gameObject) != null;

    public Node FindNodeByGameObject(GameObject gameObject) =>
      Graph.Nodes.SingleOrDefault(n => n.gameObject == gameObject);

    public bool IsEdge(GameObject gameObject) => FindEdgeByGameObject(gameObject) != null;

    public Edge FindEdgeByGameObject(GameObject gameObject) =>
      Graph.Edges.SingleOrDefault(n => n.segmentGroup.Contains(gameObject));

    public Node FindNodeById(int id) => Graph.Nodes.SingleOrDefault(n => n.Id == id);

    public void AddNode(Vector3 position, Quaternion rotation)
    {
      var id = Graph.Nodes.Any()
        ? Graph.Nodes.Max(n => n.Id) + 1
        : 0;

      var node = Node.EmptyNode(id, nodeGameObjectFactory.CreateDefaultNode(position, rotation));
      node.Position = position;
      node.Rotation = rotation.eulerAngles;
      node.Text.enabled = LabelVisibility;
      Graph.Nodes.Add(node);
    }

    public void AddEdge(Node node1, Node node2)
    {
      var id = Graph.Edges.Any()
        ? Graph.Edges.Max(n => n.Id) + 1
        : 0;

      var edge = Edge.BetweenNodes(id, null, node1, node2);
      var existingEdges = Graph.FindEdgesByNodes(node1, node2);
      foreach (var existingEdge in existingEdges)
      {
        existingEdge.segmentGroup.Destroy();
        Destroy(existingEdge.labelGameObject);
      }
      existingEdges.Add(edge);
      var x = edgeGameObjectFactory.CreateGameObjectEdgesFor(existingEdges, LabelVisibility);
      for (var i = 0; i < existingEdges.Count; i++)
      {
        var (segmentGroup, label) = x[i];
        existingEdges[i].segmentGroup = segmentGroup;
        existingEdges[i].labelGameObject = label;
      }
      Graph.Edges.Add(edge);
    }

    public void RemoveNode(Node node)
    {
      Destroy(node.gameObject);
      Graph.Nodes.Remove(node);
      var edgesToRemove = Graph.FindNodeEdges(node);

      foreach (var edge in edgesToRemove)
      {
        RemoveAllEdgesBetween(edge.nodeFrom, edge.nodeTo);
      }
    }

    public void RemoveEdge(Edge edge)
    {
      edge.segmentGroup.Destroy();
      Graph.Edges.Remove(edge);
      FixEdges();
    }

    public void RemoveAllEdgesBetween(Node node1, Node node2)
    {
      var edges = Graph.FindEdgesByNodes(node1, node2);
      foreach (var edge in edges)
      {
        edge.segmentGroup.Destroy();
        Graph.Edges.Remove(edge);
      }
    }

    /// <summary>
    /// Update edges positions based on corresponding nodes
    /// </summary>
    public void FixEdges()
    {
      foreach (var edgeGroup in Graph.GetEdgesGroupedByNodes())
      {
        var (nodeFrom, nodeTo) = GetStartAndEndNodes(edgeGroup.Key);
        FixEdgeGroup(edgeGroup.Value, nodeFrom, nodeTo);
      }
    }

    public void FixEdgesOfNode(Node node)
    {
      var edgeLists =
        Graph.GetEdgesGroupedByNodes()
          .Where(edgeGroup => edgeGroup.Key.Contains(node))
          .Select(edgeGroup => (edgeGroup.Value, GetStartAndEndNodes(edgeGroup.Key)));

      foreach (var (edges, (startingNode, endingNode)) in edgeLists)
      {
        FixEdgeGroup(edges, startingNode, endingNode);
      }
    }

    (Node startingNode, Node endingNode) GetStartAndEndNodes(HashSet<Node> edgeGroupKey)
    {
      var nodes = edgeGroupKey.ToList();
      return
        nodes.Count == 1
          ? (nodes[0], nodes[0])
          : (nodes[0], nodes[1]);
    }

    void FixEdgeGroup(IList<Edge> edges, Node nodeFrom, Node nodeTo)
    {
      var startingPosition = nodeFrom.Position;
      var endingPosition = nodeTo.Position;

      var linePositions = lineFactory.GetLinePositionsFor(startingPosition, endingPosition, edges.Count);
      for (var i = 0; i < edges.Count; i++)
      {
        edges[i].segmentGroup.PlaceAlongPoints(linePositions[i]);
      }
    }

    public void SetNodeClass(Node node, GraphElementClass nodeClass)
    {
      var newClassId = nodeClass?.Id ?? null;
      if (node.ClassId == newClassId)
        return;

      node.ClassId = newClassId;
      node.nodeClass = Graph.Classes.SingleOrDefault(c => c.Id == newClassId);
      Destroy(node.gameObject);
      node.gameObject = nodeGameObjectFactory.CreateNodeGameObject(node);
      var text = node.gameObject.GetComponentInChildren<Text>();
      text.text = node.Label;
      text.enabled = LabelVisibility;
    }
  }
}
