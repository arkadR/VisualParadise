using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Model
{
  [Serializable]
  public class Graph
  {
    public List<Node> nodes;
    public List<Edge> edges;
  }
}
