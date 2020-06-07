using Assets.GameObject;
using Assets.Model;
using Assets.Scripts;
using UnityEngine;

namespace Player.Guns
{
  public class EdgeGun : IGun
  {
    private const float hitDistance = 20f;

    private readonly GraphService graphService;
    private readonly Material edgeMaterial;
    private Node previouslyHitNode;

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
        SetGlow(previouslyHitNode, true);
        return;
      }

      //Don't create duplicate edge
      if (graphService.FindEdgeByNodes(previouslyHitNode, currentlyHitNode) != null) 
        return;

      graphService.AddEdge(currentlyHitNode, previouslyHitNode, edgeMaterial);
      SetGlow(previouslyHitNode, false);
      previouslyHitNode = null;
    }

    void SetGlow(Node node, bool value)
    {
      var nodeMaterial = node.gameObject.GetComponent<Renderer>().material;
      nodeMaterial.SetColor("_EmissionColor", Color.yellow);
      if (value)
        nodeMaterial.EnableKeyword("_EMISSION");
      else
        nodeMaterial.DisableKeyword("_EMISSION");
    }

    public void OnSwitchedAway()
    {
      // noop
    }
  }
}