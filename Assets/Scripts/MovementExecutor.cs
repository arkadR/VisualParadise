using UnityEngine;

namespace Assets.Scripts
{
  public class MovementExecutor : MonoBehaviour
  {
    GraphService graphService;

    private bool shouldMove = false;

    void Start()
    {
      graphService = FindObjectOfType<GraphService>();
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
      graphService.FixEdges();
    }

    public void ToggleMovement()
    {
      shouldMove = !shouldMove;
    
      Debug.Log(shouldMove ? "Node movement enabled" : "Node movement disabled");
      if (shouldMove == false)
        graphService.StopNodes();
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
  }
}
