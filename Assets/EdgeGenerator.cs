using Assets.Model;
using UnityEngine;

public static class EdgeGenerator
{
    public static GameObject CreateGameObjectEdge(
        Node from,
        Node to,
        Material edgeMaterial)
    {
        var line = new GameObject();
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from.point.Position());
        lineRenderer.SetPosition(1, to.point.Position());
        lineRenderer.startWidth = 0.2f;
        lineRenderer.material = edgeMaterial;
        return line;
    }
}