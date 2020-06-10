using UnityEngine;

namespace Assets.Scripts
{
  public class MovementExecutor : MonoBehaviour
  {
    GraphService graphService;

    private bool _shouldMove = false;
    private int _velocityModifier = 1;

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

    public void DisableMovement()
    {
      if (_shouldMove)
        ToggleMovement();
    }

    public void ToggleReverse()
    {
      _velocityModifier *= -1;
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
        n.Position += n.Velocity * Time.deltaTime * _velocityModifier;
        n.Rotation += n.AngularVelocity * Time.deltaTime * _velocityModifier;
      }
    }
  }
}
