using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Graph
  {
    public List<Node> nodes = new List<Node>();
    public List<Edge> edges = new List<Edge>();



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
