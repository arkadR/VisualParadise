using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Graph
  {
    [JsonProperty] public List<Node> Nodes { get; private set; } = new List<Node>();
    [JsonProperty] public List<Edge> Edges { get; private set; } = new List<Edge>();
    [JsonProperty] public List<NodeClass> NodeClasses { get; private set; } = new List<NodeClass>();
    [JsonProperty] public List<EdgeClass> EdgeClasses { get; private set; } = new List<EdgeClass>();
    
    [OnDeserialized]
    public void OnDeserializedMethod(StreamingContext context)
    {
      foreach (var edge in Edges)
      {
        edge.nodeFrom = Nodes.Single(n => n.Id == edge.From);
        edge.nodeTo = Nodes.Single(n => n.Id == edge.To);
        if (edge.ClassId != null)
          edge.Class = EdgeClasses.Single(c => c.Id == edge.ClassId);
      }

      foreach (var node in Nodes)
      {
        if (node.ClassId != null)
          node.nodeClass = NodeClasses.Single(c => c.Id == node.ClassId);
      }
    }

    public IList<Edge> FindEdgesByNodes(Node node1, Node node2) =>
      Edges
        .Where(edge =>
          (edge.nodeFrom == node1 && edge.nodeTo == node2) || 
          (edge.nodeFrom == node2 && edge.nodeTo == node1)
        ).ToList();

    public IList<Edge> FindNodeEdges(Node node) =>
      Edges.Where(e => e.From == node.Id || e.To == node.Id).ToList();

    public IDictionary<HashSet<Node>, List<Edge>> GetEdgesGroupedByNodes()
    {
      var groups = new Dictionary<HashSet<Node>, List<Edge>>();
      foreach (var edge in Edges)
      {
        var nodesSet = new HashSet<Node> {edge.nodeFrom, edge.nodeTo};
        var existing = groups
          .FirstOrDefault(x => x.Key.SetEquals(nodesSet));
        if (existing.Key == null)
          groups[nodesSet] = new List<Edge> {edge};
        else
          groups[existing.Key].Add(edge);
      }

      return groups;
    }
  }
}
