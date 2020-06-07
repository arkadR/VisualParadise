using Assets.Scripts;
using UnityEngine;

public class MovementExecutor : MonoBehaviour
{
  GraphService graphService;

  private bool movement = false;

  // Start is called before the first frame update
  // needs to be called after GraphLoader.Start()
  void Start()
  {
    graphService = FindObjectOfType<GraphService>();
    Debug.Log("Nodes count: " + graphService.physicalNodes.Count);
    Debug.Log("Edges count: " + graphService.physicalEdges.Count);
  }

  // Update for physics
  void FixedUpdate()
  {
    if (GameService.Instance.IsPaused)
      return;

    if (movement)
    {
      Accelerate();
      FixEdges();
    }
  }

  public void HandleExecuteMovementButtonPress()
  {
    movement = !movement;
    if (movement)
    {
      Move();
      Debug.Log("Execute movement on");
    }
    else
    {
      StopNodes();
      Debug.Log("Execute movement off");
    }
  }

  private void Move()
  {
    foreach (var n in graphService.physicalNodes)
    {
      Rigidbody nodeRb = n.physicalNode.GetComponent<Rigidbody>();

      nodeRb.velocity = n.node.vpoint.Velocity();
      nodeRb.angularVelocity = n.node.vpoint.AngVelocity();
    }
  }

  private void Accelerate()
  {
    foreach (var n in graphService.physicalNodes)
    {
      Rigidbody nodeRb = n.physicalNode.GetComponent<Rigidbody>();

      nodeRb.AddForce(n.node.apoint.Acceleration(), ForceMode.Acceleration);
      nodeRb.angularVelocity += n.node.apoint.AngAcceleration() * Time.deltaTime;

      n.node.point.SetPosition(nodeRb.position);
      n.node.vpoint.SetVelocity(nodeRb.velocity);
      n.node.vpoint.SetAngVelocity(nodeRb.angularVelocity);
    }
  }

  /// <summary>
  /// Update edges position based on corresponding nodes
  /// </summary>
  public void FixEdges()
  {
    foreach (var e in graphService.physicalEdges)
    {
      var lineRenderer = e.edge.GetComponent<LineRenderer>();
      var positionFrom = e.nodeFrom.physicalNode.GetComponent<Rigidbody>().position;
      var positionTo = e.nodeTo.physicalNode.GetComponent<Rigidbody>().position;
      lineRenderer.SetPosition(0, positionFrom);
      lineRenderer.SetPosition(1, positionTo);
    }
  }

  /// <summary>
  /// Set velocity of all nodes to 0
  /// </summary>
  public void StopNodes()
  {
    foreach (var n in graphService.physicalNodes)
    {
      Rigidbody nodeRb = n.physicalNode.GetComponent<Rigidbody>();

      nodeRb.velocity = Vector3.zero;
      nodeRb.angularVelocity = Vector3.zero;
    }
  }
}