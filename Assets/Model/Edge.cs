using System;

namespace Assets.Model
{
  [Serializable]
  public class Edge
  {
    public int from;
    public int to;
    public int weight;

    [NonSerialized] public UnityEngine.GameObject gameObject;

    [NonSerialized] public Node nodeFrom;
    [NonSerialized] public Node nodeTo;
  }
}