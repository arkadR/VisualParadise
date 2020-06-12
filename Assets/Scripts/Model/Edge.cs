using System;
using Assets.Scripts.Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

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

    public Text Text => gameObject.GetComponentInChildren<Text>();

    public string DefaultLabel 
    {
      get => $"{from}-{to}";
    }

    public void UpdateTextPosition()
    {
      var (x1, y1, _) = Camera.main.WorldToScreenPoint(nodeFrom.Position);
      var (x2, y2, _) = Camera.main.WorldToScreenPoint(nodeTo.Position);
      var tan = (y1 - y2) / (x1 - x2);
      var angle = Mathf.Atan(tan) * Mathf.Rad2Deg;
      var centerOfEdgeOnScreen = new Vector2(x1 + x2, y1 + y2) / 2;
      Text.SetPositionOnScreen(centerOfEdgeOnScreen);
      Text.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
  }
}
