using Assets.Model;
using UnityEngine;

public static class EdgeGenerator
{
  public static GameObject CreateGameObjectEdge(
      Node fromNode,
      Node toNode,
      Material edgeMaterial)
  {
    var line = new GameObject();
    var lineRenderer = line.AddComponent<LineRenderer>();
    lineRenderer.SetPosition(0, fromNode.Position);
    lineRenderer.SetPosition(1, toNode.Position);
    lineRenderer.startWidth = 0.2f;
    lineRenderer.material = edgeMaterial;
    return line;
  }
}