using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Graph
  {
    public List<Edge> edges;
    public List<Node> nodes;

    public Node FindNodeById(int id) => nodes.SingleOrDefault(n => n.id == id);
  }
}
