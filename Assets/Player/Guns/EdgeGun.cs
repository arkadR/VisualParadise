using Assets.GameObject;
using Assets.Scripts;
using UnityEngine;

namespace Player.Guns
{
  public class EdgeGun : IGun
  {
    private float hitDistance = 20f;
    private readonly GraphService graphService;
    private readonly Material edgeMaterial;
    private PhysicalNode previouslyHitNode;

    public EdgeGun(GraphService graphService, Material edgeMaterial)
    {
      this.graphService = graphService;
      this.edgeMaterial = edgeMaterial;
    }

    public string GunName => "Edge";

    public void OnMoveDown(Transform playerTransform, Camera camera)
    {
      var transform = camera.transform;
      bool hit = Physics.Raycast(
          transform.position,
          transform.forward,
          out var hitInfo,
          hitDistance);
      if (!hit) 
        return;
      
      var gameObjectHit = hitInfo.collider.gameObject;
      if (IsNotPhysicalNode(gameObjectHit)) return;
      if (HitTheSamePhysicalNode(gameObjectHit)) return;

      var currentlyHitPhysicalNode = graphService.FindPhysicalNodeByGameObject(gameObjectHit);

      if (previouslyHitNode == null)
      {
        previouslyHitNode = currentlyHitPhysicalNode;
        SetGlow(previouslyHitNode, true);
        return;
      }

      if (EdgeOfNodesAlreadyExists(previouslyHitNode, currentlyHitPhysicalNode)) 
        return;

      graphService.AddEdge(currentlyHitPhysicalNode.node, previouslyHitNode.node, edgeMaterial);
      SetGlow(previouslyHitNode, false);
      previouslyHitNode = null;
    }

    void SetGlow(PhysicalNode physicalNode, bool value)
    {
      Material physicalNodeMaterial = physicalNode.physicalNode.GetComponent<Renderer>().material;
      physicalNodeMaterial.SetColor("_EmissionColor", Color.yellow);
      if (value)
      {
        physicalNodeMaterial.EnableKeyword("_EMISSION");
      }
      else
      {
        physicalNodeMaterial.DisableKeyword("_EMISSION");
      }
    }


    private bool EdgeOfNodesAlreadyExists(PhysicalNode first, PhysicalNode second)
    {
      var alreadyExistingEdge = graphService
          .FindPhysicalEdgeByPhysicalNodes(first, second);
      return alreadyExistingEdge != null;
    }

    private bool HitTheSamePhysicalNode(GameObject gameObjectHit)
    {
      if (previouslyHitNode == null) return false;
      return gameObjectHit == previouslyHitNode.physicalNode;
    }

    private static bool IsNotPhysicalNode(GameObject gameObjectHit)
    {
      return !gameObjectHit.CompareTag(Constants.PhysicalNodeTag);
    }

    public void OnSwitchedAway()
    {
      // noop
    }
  }
}