using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Graph
  {
    public List<Node> nodes;
    public List<Edge> edges;



    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
      if (nodes == null)
        nodes = new List<Node>();

      if (edges == null)
        edges = new List<Edge>();
    }
  }
}
