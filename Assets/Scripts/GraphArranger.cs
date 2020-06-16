using System.Linq;
using Assets.Scripts.Common.Utils;
using UnityEngine;

namespace Assets.Scripts
{
  public class GraphArranger : MonoBehaviour
  {
    const float _repelFunCoefficient = 5.0f; // higher values causes more distortion
    const float _attractFunPower = 1.5f; // safe range <1, 3>
    const float _maxVelocityMagnitude = 25f;
    public GraphArrangerMode ArrangeMode { get; private set; }  = default;
    GraphService _graphService;

    public void Start() => _graphService = FindObjectOfType<GraphService>();

    public void FixedUpdate()
    {
      if (GameService.Instance.IsPaused)
        return;

      if (!ArrangeEnabled)
        return;

      Attract();
      Repel();
      if (ArrangeMode == GraphArrangerMode._2D)
        SquashTo2D();
      _graphService.FixEdges();
    }

    public bool ArrangeEnabled { get; private set; }

    public void ToggleArrangement()
    {
      ArrangeEnabled = !ArrangeEnabled;
      Debug.Log("Arranger " + (ArrangeEnabled ? "enabled" : "disabled"));
    }

    public void ToggleMode() => ArrangeMode = (GraphArrangerMode)EnumUtils<GraphArrangerMode>.GetNextValue((int)ArrangeMode);

    public void DisableArrangement()
    {
      if (ArrangeEnabled)
        ToggleArrangement();
    }

    /// <summary>
    ///   Repel each Node from every other node
    /// </summary>
    void Repel()
    {
      for (var i = 0; i < _graphService.Graph.nodes.Count; i++)
      {
        for (var j = i + 1; j < _graphService.Graph.nodes.Count; j++)
        {
          var node1 = _graphService.Graph.nodes[i];
          var node2 = _graphService.Graph.nodes[j];

          var direction = node1.Position - node2.Position;
          var distance = direction.magnitude;

          var velocityMagnitude = CalculateRepelVelocityMagnitude(distance);
          var velocity = direction.normalized * Mathf.Min(_maxVelocityMagnitude, velocityMagnitude);

          node1.Position += velocity * Time.deltaTime;
          node2.Position -= velocity * Time.deltaTime;
        }
      }
    }

    float CalculateRepelVelocityMagnitude(float distance) => _repelFunCoefficient / distance;

    /// <summary>
    ///   Attract two nodes if there is an edge to connect them
    /// </summary>
    void Attract()
    {
      var uniqueEdges = _graphService.Graph.edges
        .GroupBy(e => new {e.from, e.to})
        .Select(g => g.First());

      foreach (var e in uniqueEdges)
      {
        var node1 = _graphService.FindNodeById(e.from);
        var node2 = _graphService.FindNodeById(e.to);

        var direction = node1.Position - node2.Position;
        var distance = direction.magnitude;

        var velocityMagnitude = CalculateAttractVelocityMagnitude(distance);
        var velocity = direction.normalized * Mathf.Min(_maxVelocityMagnitude, velocityMagnitude);

        node1.Position -= velocity * Time.deltaTime;
        node2.Position += velocity * Time.deltaTime;
      }
    }

    float CalculateAttractVelocityMagnitude(float distance) => Mathf.Pow(distance, _attractFunPower);

    private void SquashTo2D()
    {
      float avgY = _graphService.Graph.nodes.Average(n => n.Position.y);
      foreach(var n in _graphService.Graph.nodes)
      {
        float distanceY = Mathf.Abs(n.Position.y - avgY);
        if (distanceY < 0.005)
          continue;
        var newPosition = n.Position;
        var shrinkMultiplier = distanceY > 1 ? 8 : 20;
        newPosition.y = Mathf.Lerp(n.Position.y, avgY, Time.deltaTime * shrinkMultiplier);
        n.Position = newPosition;
      }
    }
  }
}
