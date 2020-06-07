using System.IO;
using Assets.Model;
using Assets.Scripts;
using UnityEngine;

public class GraphLoader : MonoBehaviour
{

  public GraphService graphService;

  void Start()
  {
    var graphFilePath = PlayerPrefs.GetString("graphFilePath");
    var graph = LoadGraph(graphFilePath);
    graphService.SetGraph(graph);
  }

  private static Graph LoadGraph(string filePath)
  {
    var graphJson = File.ReadAllText(filePath);
    return JsonUtility.FromJson<Graph>(graphJson);
  }
}