using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class GraphLoader : MonoBehaviour
  {
    public GraphService graphService;

    void Start()
    {
      var graphFilePath = PlayerPrefs.GetString(Constants.GraphFilePathKey);
      var graph = LoadGraph(graphFilePath) ?? new Graph();

      if (graph.nodes == null)
        graph.nodes = new List<Node>();

      if (graph.edges == null)
        graph.edges = new List<Edge>();

      graphService.SetGraph(graph);
    }

    private static Graph LoadGraph(string filePath)
    {
      //TODO: Try/catch file for bad formats.
      var graphJson = File.ReadAllText(filePath);
      var graph = JsonUtility.FromJson<Graph>(graphJson);
      Debug.Log($"Nodes count: {graph?.nodes.Count}\nEdges count: {graph?.edges.Count}");
      return graph;
    }
  }
}
