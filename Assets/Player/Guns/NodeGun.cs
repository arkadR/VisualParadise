using Assets.Scripts;
using UnityEngine;

namespace Player.Guns
{
  public class NodeGun : IGun
  {
    private readonly GraphService graphService;
    private readonly Material nodeMaterial;

    public NodeGun(GraphService graphService, Material nodeMaterial)
    {
      this.graphService = graphService;
      this.nodeMaterial = nodeMaterial;
    }

    public string GetGunName() => "Node";

    public void OnMoveDown(Transform playerTransform, Camera camera)
    {
      var transform = camera.transform;
      var rotation = transform.rotation;
      var position = transform.position + transform.forward * 3;

      graphService.AddNode(position, rotation, nodeMaterial);
    }

    public void OnSwitchedAway()
    {
      //noop
    }
  }
}