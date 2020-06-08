using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public static class EdgeGenerator
  {
    public static UnityEngine.GameObject CreateGameObjectEdge(
      Node fromNode,
      Node toNode,
      Material edgeMaterial)
    {
      var line = new UnityEngine.GameObject();
      var lineRenderer = line.AddComponent<LineRenderer>();
      lineRenderer.SetPosition(0, fromNode.Position);
      lineRenderer.SetPosition(1, toNode.Position);
      lineRenderer.startWidth = 0.2f;
      lineRenderer.material = edgeMaterial;
      return line;
    }
  }
}
