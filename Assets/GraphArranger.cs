using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphArranger : MonoBehaviour
{
    GraphLoader graphLoader;
    const float equilibriumDistance = 8.0f;
    const float equilibriumDistance2 = equilibriumDistance * equilibriumDistance;

    private bool arrange = false;

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

    void HandleArrangeButtonPress()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            arrange = !arrange;
            Debug.Log("Presenter " + (arrange ? "on" : "off"));
            StopNodes();
        }
    }

    /// <summary>
    /// Repell each Node from every other node
    /// v(d) = equilibrium2 / d
    /// </summary>
    void Repell()
    {
        foreach (var n in graphLoader.physicalNodes)
        {
            Rigidbody nodeRb = n.node.GetComponent<Rigidbody>();
            foreach (var o in graphLoader.physicalNodes)
            {
                if (n == o) continue;
                Rigidbody otherRb = o.node.GetComponent<Rigidbody>();

                Vector3 direction = nodeRb.position - otherRb.position;
                float distance = direction.magnitude;

                float velocityMagnitude = equilibriumDistance2 / distance;
                Vector3 velocity = direction.normalized * velocityMagnitude;
                otherRb.velocity += -velocity;
            }
        }
    }

    /// <summary>
    /// Attract two nodes if there is an edge to connect them
    /// v(d) = d
    /// </summary>
    void Attract()
    {
        foreach (var e in graphLoader.physicalEdges)
        {
            Rigidbody fromRb = e.nodeFrom.node.GetComponent<Rigidbody>();
            Rigidbody toRb = e.nodeTo.node.GetComponent<Rigidbody>();

            Vector3 direction = fromRb.position - toRb.position;
            float distance = direction.magnitude;

            Vector3 velocity = direction.normalized * distance;

            fromRb.velocity += -velocity;
            toRb.velocity += velocity;
        }
    }

    /// <summary>
    /// Update edges position based on corresponding nodes
    /// </summary>
    void FixEdges()
    {
        foreach(var e in graphLoader.physicalEdges)
        {
            var lineRenderer = e.edge.GetComponent<LineRenderer>();
            var positionFrom = e.nodeFrom.node.GetComponent<Rigidbody>().position;
            var positionTo = e.nodeTo.node.GetComponent<Rigidbody>().position;
            lineRenderer.SetPosition(0, positionFrom);
            lineRenderer.SetPosition(1, positionTo);
        }
    }

    /// <summary>
    /// Set velocity of all nodes to 0
    /// </summary>
    void StopNodes()
    {
        foreach(var n in graphLoader.physicalNodes)
        {
            Rigidbody nodeRb = n.node.GetComponent<Rigidbody>();
            nodeRb.velocity = new Vector3(0, 0, 0);
        }
    }
}


