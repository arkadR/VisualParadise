using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
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

      foreach (var edge in Graph.edges)
      {
        edge.nodeFrom = FindNodeById(edge.from);
        edge.nodeTo = FindNodeById(edge.to);
      }

      foreach (var edgeGroup in Graph.GetEdgesGroupedByNodes().Values)
      {
        edgeGameObjectFactory.CreateGameObjectEdgesFor(edgeGroup, LabelVisibility);
      }

      foreach (var edge in Graph.edges)
      {
        edge.gameObject.GetComponentInChildren<Text>().text = edge.label;
      }

      LabelVisibility = false;
    }

    public bool IsNode(GameObject gameObject) => FindNodeByGameObject(gameObject) != null;

    public Node FindNodeByGameObject(GameObject gameObject) =>
      Graph.nodes.SingleOrDefault(n => n.gameObject == gameObject);

    public bool IsEdge(GameObject gameObject) => FindEdgeByGameObject(gameObject) != null;

    public Edge FindEdgeByGameObject(GameObject gameObject) =>
      Graph.edges.SingleOrDefault(n => n.gameObject == gameObject);

    public Node FindNodeById(int id) => Graph.nodes.SingleOrDefault(n => n.id == id);

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
      edge.label = edge.DefaultLabel;
      var existingEdges = Graph.FindEdgesByNodes(node1, node2);
      existingEdges.Add(edge);
      edgeGameObjectFactory.CreateGameObjectEdgesFor(existingEdges,LabelVisibility);
      Graph.edges.Add(edge);
    }

    public void RemoveNode(Node node)
    {
      Destroy(node.gameObject);
      Graph.nodes.Remove(node);
      var edgesToRemove = Graph.FindNodeEdges(node);

      foreach (var edge in edgesToRemove)
      {
        RemoveAllEdgesBetween(edge.nodeFrom, edge.nodeTo);
      }
    }

    public void RemoveEdge(Edge edge)
    {
      Destroy(edge.gameObject);
      Graph.edges.Remove(edge);
      FixEdges();
    }

    public void RemoveAllEdgesBetween(Node node1, Node node2)
    {
      var edges = Graph.FindEdgesByNodes(node1, node2);
      foreach (var edge in edges)
      {
        Destroy(edge.gameObject);
        Graph.edges.Remove(edge);
      }
    }

    /// <summary>
    ///   Update edges positions based on corresponding nodes
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
        var linePositionsCount = linePositions[i].Count;
        var lineRenderer = edges[i].gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = linePositionsCount;
        lineRenderer.SetPositions(linePositions[i].ToArray());

        UpdateEdgeColliders(edges[i], linePositions[i]);
      }
    }

    void UpdateEdgeColliders(Edge edge, IList<Vector3> linePositions)
    {
      var edgeColliderTransforms = edge.gameObject.GetComponentsInChildren<Transform>()
        .Where(t => t.gameObject.name == Constants.ColliderGameObjectName).ToList();

      for (int i = 0; i < linePositions.Count; i++)
      {
        if (i > 0)
        {
          var collider = edgeColliderTransforms[i - 1];
          var capsule = collider.GetComponent<CapsuleCollider>();
          capsule.radius = 0.09f;
          capsule.height = Vector3.Distance(linePositions[i], linePositions[i - 1]);
          capsule.direction = 2;

          collider.position = (linePositions[i - 1] + linePositions[i]) * 0.5f;
          var direction = linePositions[i] - linePositions[i - 1];
          collider.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
      }
    }
  }
}
