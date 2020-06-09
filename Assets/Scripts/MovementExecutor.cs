using UnityEngine;

namespace Assets.Scripts
{
  public class MovementExecutor : MonoBehaviour
  {
    GraphService graphService;

    private bool _shouldMove = false;
    private bool _reverse = false;

    void Start()
    {
      graphService = FindObjectOfType<GraphService>();
    }

    // Update for physics
    void FixedUpdate()
    {
      if (GameService.Instance.IsPaused)
        return;

      if (_shouldMove == false)
        return;

      Move();
      Accelerate();
      graphService.FixEdges();
    }

    public void ToggleMovement()
    {
      _shouldMove = !_shouldMove;
      Debug.Log("Node movement " + (_shouldMove ? "enabled" : "disabled"));
    }

    public void ToggleReverse()
    {
      _reverse = !_reverse;
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
        if (_reverse)
        {
          n.Position -= n.Velocity * Time.deltaTime;
          n.Rotation -= n.AngularVelocity * Time.deltaTime;
        }
        else
        {
          n.Position += n.Velocity * Time.deltaTime;
          n.Rotation += n.AngularVelocity * Time.deltaTime;
        }
      }
    }
  }
}
