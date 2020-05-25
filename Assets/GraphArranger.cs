using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphArranger : MonoBehaviour
{
    GraphLoader graphLoader;
    public float repellFunCoefficient = 5.0f;   // safe range <1, 50+)
    public float attractFunPower = 1.5f;        // safe range <1, 1.5>

    private bool arrange = false;

    // Start is called before the first frame update
    // needs to be called after GraphLoader.Start()
    void Start()
    {
        graphLoader = GameObject.FindObjectOfType<GraphLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleArrangeButtonPress();
    }

    // Update for physics
    void FixedUpdate()
    {
        if (arrange)
        {
            StopNodes();
            Repell();
            Attract();
            FixEdges();
        }
    }

    private void HandleArrangeButtonPress()
    {
        if (Input.GetButtonDown("Arrange"))
        {
            arrange = !arrange;
            Debug.Log("Arranger " + (arrange ? "on" : "off"));
            StopNodes();
        }
    }

    /// <summary>
    /// Repell each Node from every other node
    /// </summary>
    private void Repell()
    {
        foreach (var n in graphLoader.physicalNodes)
        {
            Rigidbody nodeRb = n.physicalNode.GetComponent<Rigidbody>();
            foreach (var o in graphLoader.physicalNodes)
            {
                if (n == o) continue;
                Rigidbody otherRb = o.physicalNode.GetComponent<Rigidbody>();

                Vector3 direction = nodeRb.position - otherRb.position;
                float distance = direction.magnitude;

                float velocityMagnitude = CalculateRepellVelocityMagnitude(distance);
                Vector3 velocity = direction.normalized * velocityMagnitude;
                otherRb.velocity += -velocity;
            }
        }
    }

    private float CalculateRepellVelocityMagnitude(float distance)
    {
        float rawResult = repellFunCoefficient / distance;
        return rawResult;
    }

    /// <summary>
    /// Attract two nodes if there is an edge to connect them
    /// </summary>
    private void Attract()
    {
        foreach (var e in graphLoader.physicalEdges)
        {
            Rigidbody fromRb = e.nodeFrom.physicalNode.GetComponent<Rigidbody>();
            Rigidbody toRb = e.nodeTo.physicalNode.GetComponent<Rigidbody>();

            Vector3 direction = fromRb.position - toRb.position;
            float distance = direction.magnitude;

            Vector3 velocity = direction.normalized * CalculateAttractVelocityMagnitude(distance);

            fromRb.velocity += -velocity;
            toRb.velocity += velocity;
        }
    }

    private float CalculateAttractVelocityMagnitude(float distance)
    {
        float rawResult = Mathf.Pow(distance, attractFunPower);
        return rawResult;
    }

    /// <summary>
    /// Update edges position based on corresponding nodes
    /// </summary>
    private void FixEdges()
    {
        foreach(var e in graphLoader.physicalEdges)
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
    private void StopNodes()
    {
        foreach(var n in graphLoader.physicalNodes)
        {
            Rigidbody nodeRb = n.physicalNode.GetComponent<Rigidbody>();
            nodeRb.velocity = new Vector3(0, 0, 0);
        }
    }
}


