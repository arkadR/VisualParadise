using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementExecutor : MonoBehaviour
{
    GraphLoader graphLoader;

    private bool movement = false;

    // Start is called before the first frame update
    // needs to be called after GraphLoader.Start()
    void Start()
    {
        graphLoader = GameObject.FindObjectOfType<GraphLoader>();
        Debug.Log("Nodes count: " + graphLoader.physicalNodes.Count);
        Debug.Log("Edges count: " + graphLoader.physicalEdges.Count);
    }

    // Update is called once per frame
    void Update()
    {
        HandleExecuteMovementButtonPress();
    }

    // Update for physics
    void FixedUpdate()
    {
        if (movement)
        {
            Accelerate();
            FixEdges();
        }
    }

    private void HandleExecuteMovementButtonPress()
    {
        if (Input.GetButtonDown("Move"))
        {
            movement = !movement;
            if (movement)
            {
                Move();
                Debug.Log("Exectute movement on");
            }
            else
            {
                StopNodes();
                Debug.Log("Exectute movement off");
            }
        }
    }

    private void Move()
    {
        foreach (var n in graphLoader.physicalNodes)
        {
            Rigidbody nodeRb = n.physicalNode.GetComponent<Rigidbody>();

            nodeRb.velocity = n.node.vpoint.Velocity();
            nodeRb.angularVelocity = n.node.vpoint.AngVelocity();
        }
    }

    private void Accelerate()
    {
        foreach (var n in graphLoader.physicalNodes)
        {
            Rigidbody nodeRb = n.physicalNode.GetComponent<Rigidbody>();

            nodeRb.AddForce(n.node.apoint.Acceleration(), ForceMode.Acceleration);
            nodeRb.angularVelocity += n.node.apoint.AngAcceleration() * Time.deltaTime;
        }
    }

    /// <summary>
    /// Update edges position based on corresponding nodes
    /// </summary>
    public void FixEdges()
    {
        foreach (var e in graphLoader.physicalEdges)
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
        foreach (var n in graphLoader.physicalNodes)
        {
            Rigidbody nodeRb = n.physicalNode.GetComponent<Rigidbody>();

            nodeRb.velocity = Vector3.zero;
            nodeRb.angularVelocity = Vector3.zero;
        }
    }
}
