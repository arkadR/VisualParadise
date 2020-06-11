using UnityEngine;

namespace Assets.Scripts
{
  public class MovementExecutor : MonoBehaviour
  {
    bool _shouldMove;
    int _velocityModifier = 1;
    GraphService graphService;

    public void Start() => graphService = FindObjectOfType<GraphService>();

    // Update for physics
    public void FixedUpdate()
    {
      if (GameService.Instance.IsPaused)
        return;

      if (_shouldMove == false)
      {
        UpdateLabelPositions();
        return;
      }

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

    public void ToggleReverse() => _velocityModifier *= -1;

    void Accelerate()
    {
      foreach (var n in graphService.Graph.nodes)
      {
        var newVelocity = n.Velocity + (n.Acceleration * Time.deltaTime);
        var newAngularVelocity = n.AngularVelocity + (n.AngularAcceleration * Time.deltaTime);

        n.Velocity = newVelocity;
        n.AngularVelocity = newAngularVelocity;
      }
    }

    void Move()
    {
      foreach (var n in graphService.Graph.nodes)
      {
        n.Position += n.Velocity * Time.deltaTime * _velocityModifier;
        n.Rotation += n.AngularVelocity * Time.deltaTime * _velocityModifier;
      }
    }

    /// <summary>
    /// Forces the text to update regardless if movement is on or off. Without that, text is stuck on screen in one place.
    /// </summary>
    void UpdateLabelPositions()
    {
      graphService.Graph.nodes.ForEach(n => n.UpdateText());
      graphService.Graph.edges.ForEach(e => e.UpdatePosition());
    }
  }
}
