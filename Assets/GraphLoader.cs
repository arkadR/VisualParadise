using System.Collections.Generic;
using System.IO;
using Assets.Model;
using Assets.Scripts;
using UnityEngine;

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
    return JsonUtility.FromJson<Graph>(graphJson);
  }
}