using UnityEngine;

namespace Assets.Scripts
{
  public class MovementExecutor : MonoBehaviour
  {
    GraphService graphService;

    private bool shouldMove = false;

    // Start is called before the first frame update
    // needs to be called after GraphLoader.Start()
    void Start()
    {
      graphService = FindObjectOfType<GraphService>();
      Debug.Log("Nodes count: " + graphService.Graph.nodes.Count);
      Debug.Log("Edges count: " + graphService.Graph.edges.Count);
    }

    // Update for physics
    void FixedUpdate()
    {
      if (GameService.Instance.IsPaused)
        return;

      if (shouldMove == false)
        return;

      Move();
      Accelerate();
      FixEdges();
    }

    public void ToggleMovement()
    {
      shouldMove = !shouldMove;
    
      Debug.Log(shouldMove ? "Node movement enabled" : "Node movement disabled");
      if (shouldMove == false)
        StopNodes();
    }



    private void Accelerate()
    {
      foreach (var n in graphService.Graph.nodes)
      {
        var newVelocity = n.Velocity + n.Acceleration * Time.deltaTime;
        var newAngularVelocity = n.AngularVelocity + n.AngularAcceleration * Time.deltaTime;

        n.Velocity = newVelocity;
        n.AngularVelocity = newAngularVelocity;
      }
    }

    private void Move()
    {
      foreach (var n in graphService.Graph.nodes)
      {
        var newPosition = n.Position + n.Velocity * Time.deltaTime;
        var newRotation = n.Rotation + n.AngularVelocity * Time.deltaTime;

        n.Position = newPosition;
        n.Rotation = newRotation;
      }
    }

    /// <summary>
    /// Update edges position based on corresponding nodes
    /// </summary>
    public void FixEdges()
    {
      foreach (var e in graphService.Graph.edges)
      {
        graphService.FixEdge(e);
      }
    }

    /// <summary>
    /// Set velocity of all nodes to 0
    /// </summary>
    public void StopNodes()
    {
      foreach (var n in graphService.Graph.nodes)
      {
        n.Velocity = Vector3.zero;
        n.AngularVelocity = Vector3.zero;
        n.Acceleration = Vector3.zero;
        n.AngularAcceleration = Vector3.zero;
      }
    }
  }
}
