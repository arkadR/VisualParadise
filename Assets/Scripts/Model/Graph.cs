using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Graph
  {
    public List<Node> nodes;
    public List<Edge> edges;
  }
}
