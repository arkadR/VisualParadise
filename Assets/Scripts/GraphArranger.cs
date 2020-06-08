using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
  public class GraphArranger : MonoBehaviour
  {
    private GraphService graphService;

    public const float repelFunCoefficient = 5.0f; // higher values causes more distortion
    public const float attractFunPower = 1.5f; // safe range <1, 3>
    public const float maxVelocityMagnitude = 40f;

    public bool shouldArrange { get; private set; } = false;

    void Start()
    {
      graphService = UnityEngine.GameObject.FindObjectOfType<GraphService>();
    }

    // Update for physics
    void FixedUpdate()
    {
      if (GameService.Instance.IsPaused)
        return;

      if (!shouldArrange) 
        return;

      graphService.StopNodes();
      Attract();
      Repel();
      graphService.FixEdges();
    }

    public void ToggleArrangement()
    {
      shouldArrange = !shouldArrange;
      Debug.Log("Arranger " + (shouldArrange ? "enabled" : "disabled"));
      graphService.StopNodes();
    }

    /// <summary>
    /// Repel each Node from every other node
    /// </summary>
    private void Repel()
    {
      for (var i = 0; i < graphService.Graph.nodes.Count; i++)
      {
        for (var j = i+1; j < graphService.Graph.nodes.Count; j++)
        {
          var node1 = graphService.Graph.nodes[i];
          var node2 = graphService.Graph.nodes[j];

          var direction = node1.Position - node2.Position;
          var distance = direction.magnitude;

          var velocityMagnitude = CalculateRepelVelocityMagnitude(distance);
          var velocity = direction.normalized * Mathf.Min(maxVelocityMagnitude, velocityMagnitude);

          node1.Position += velocity * Time.deltaTime;
          node2.Position -= velocity * Time.deltaTime;
        }
      }
    }

    private float CalculateRepelVelocityMagnitude(float distance)
    {
      var rawResult = repelFunCoefficient / distance;
      return rawResult;
    }

    /// <summary>
    /// Attract two nodes if there is an edge to connect them
    /// </summary>
    private void Attract()
    {
      foreach (var e in graphService.Graph.edges)
      {
        var node1 = graphService.Graph.nodes.Single(n => n.id == e.from);
        var node2 = graphService.Graph.nodes.Single(n => n.id == e.to);

        var direction = node1.Position - node2.Position;
        var distance = direction.magnitude;

        var velocityMagnitude = CalculateAttractVelocityMagnitude(distance);
        var velocity = direction.normalized * Mathf.Min(maxVelocityMagnitude, velocityMagnitude);

        node1.Position -= velocity * Time.deltaTime;
        node2.Position += velocity * Time.deltaTime;
      }
    }

    private float CalculateAttractVelocityMagnitude(float distance)
    {
      var rawResult = Mathf.Pow(distance, attractFunPower);
      return rawResult;
    }
  }
}
