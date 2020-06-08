using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
  public class GraphArranger : MonoBehaviour
  {
    private GraphService graphService;
    private MovementExecutor movementExecutor;

    public const float repelFunCoefficient = 5.0f; // safe range <1, 50+)
    public const float attractFunPower = 1.5f; // safe range <1, 1.5>
    public const float maxVelocityMagnitude = 25f;

    private bool shouldArrange = false;

    // Start is called before the first frame update
    // needs to be called after GraphLoader.Start() and after MovementExecutor.Start()
    void Start()
    {
      graphService = UnityEngine.GameObject.FindObjectOfType<GraphService>();
      movementExecutor = UnityEngine.GameObject.FindObjectOfType<MovementExecutor>();
    }

    // Update for physics
    void FixedUpdate()
    {
      if (GameService.Instance.IsPaused)
        return;

      if (!shouldArrange) 
        return;

      movementExecutor.StopNodes();
      Attract();
      Repel();
    }

    public void ToggleArrangement()
    {
      shouldArrange = !shouldArrange;
      Debug.Log("Arranger " + (shouldArrange ? "on" : "off"));
      movementExecutor.StopNodes();
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
          node1.Velocity += velocity;
          node2.Velocity -= velocity;
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

        node1.Velocity -= velocity;
        node2.Velocity += velocity;
      }
    }

    private float CalculateAttractVelocityMagnitude(float distance)
    {
      var rawResult = Mathf.Pow(distance, attractFunPower);
      return rawResult;
    }
  }
}
