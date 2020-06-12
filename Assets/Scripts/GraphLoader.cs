using System.IO;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class GraphLoader : MonoBehaviour
  {
    public GraphService graphService;

    public void Start()
    {
      var graphFilePath = PlayerPrefs.GetString(Constants.GraphFilePathKey);
      var graph = LoadGraph(graphFilePath) ?? new Graph();

      graph.nodes.ForEach(n =>
      {
        if (string.IsNullOrEmpty(n.label))
          n.label = n.DefaultLabel;
      });

      graph.edges.ForEach(e =>
      {
        if (string.IsNullOrEmpty(e.label))
          e.label = e.DefaultLabel;

        e.nodeFrom = graph.nodes.Single(n => n.id == e.from);
        e.nodeTo = graph.nodes.Single(n => n.id == e.to);
      });

      Debug.Log($"Nodes count: {graph.nodes.Count}\nEdges count: {graph.edges.Count}");
      graphService.SetGraph(graph);
    }

    private static Graph LoadGraph(string filePath)
    {
      //TODO: Try/catch file for bad formats.
      var graphJson = File.ReadAllText(filePath);
      var graph = JsonSerializer.Deserialize<Graph>(graphJson);
      return graph;
    }
  }
}
