using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GraphLoader : MonoBehaviour
{

  // Start is called before the first frame update
  void Start()
  {
    var graph = LoadGraph("graph1");
    foreach (var node in graph.nodes)
    {
      var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      sphere.transform.position = node.position;
    }

    var material = Resources.Load<Material>("Edge Material");
    foreach (var edge in graph.edges)
    {
      var node1 = graph.nodes.Single(n => n.id == edge.from);
      var node2 = graph.nodes.Single(n => n.id == edge.to);
      var line = new GameObject();
      var lineRenderer = line.AddComponent<LineRenderer>();
      lineRenderer.SetPosition(0, node1.position);
      lineRenderer.SetPosition(1, node2.position);
      lineRenderer.startWidth = 0.2f;
      lineRenderer.material = material;
    }
  }

  // Update is called once per frame
  void Update()
  {

  }


  Graph LoadGraph(string fileName)
  {
    var graphJson = Resources.Load(fileName).ToString();
    return JsonUtility.FromJson<Graph>(graphJson);
  }

  [Serializable]
  class Graph
  {
    public List<Node> nodes;
    public List<Edge> edges;
  }

  [Serializable]
  class Node
  {
    public int id;
    public Vector3 position;
  }

  [Serializable]
  class Edge
  {
    public int from;
    public int to;
    public int weight;
  }
}
