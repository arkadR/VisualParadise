using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Edge
  {
    public int id;
    public string label;
    public int from;
    public int to;
    public int weight;

    [NonSerialized] public GameObject gameObject;

    [NonSerialized] public Node nodeFrom;
    [NonSerialized] public Node nodeTo;
  }
}
