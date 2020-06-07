using Assets.Model;
using Assets.Scripts;
using UnityEngine;

namespace Player.Guns
{
  public class EdgeGun : IGun
  {
    [SerializeField] private float hitDistance = 40f;
    private readonly GraphService graphService;
    private readonly Material edgeMaterial;
    private Node previouslyHitNode;
    private float nodeHitGlowStrength = 0.5f;

    public EdgeGun(GraphService graphService, Material edgeMaterial)
    {
      this.graphService = graphService;
      this.edgeMaterial = edgeMaterial;
    }

    public string GunName => "Edge";

    public void OnMoveDown(Transform playerTransform, Camera camera)
    {
      var transform = camera.transform;
      var hit = Physics.Raycast(
          transform.position,
          transform.forward,
          out var hitInfo,
          hitDistance);

      if (!hit) 
        return;
      
      var gameObjectHit = hitInfo.collider.gameObject;
      var currentlyHitNode = graphService.FindNodeByGameObject(gameObjectHit);

      // If not a node, don't do anything
      if (currentlyHitNode == null) 
        return;

      // If player hit the same node twice, don't do anything
      if (currentlyHitNode == previouslyHitNode) 
        return;


      if (previouslyHitNode == null)
      {
        previouslyHitNode = currentlyHitNode;
        previouslyHitNode.gameObject.EnableGlow();
        previouslyHitNode.gameObject.SetGlow(nodeHitGlowStrength);
        return;
      }

      //Don't create duplicate edge
      if (graphService.FindEdgeByNodes(previouslyHitNode, currentlyHitNode) != null) 
        return;

      graphService.AddEdge(currentlyHitNode, previouslyHitNode, edgeMaterial);
      previouslyHitNode.gameObject.DisableGlow();
      previouslyHitNode = null;
    }

    public void OnSwitchedAway()
    {
      // noop
    }

    public void OnRightClick(Camera camera)
    {
        
    }
    }
}