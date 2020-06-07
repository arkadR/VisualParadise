using UnityEngine;

namespace Player.Guns
{
    public class EdgeGun : IGun
    {
        [SerializeField] private float hitDistance = 40f;
        private readonly GraphLoader graphLoader;
        private readonly Material edgeMaterial;
        private GraphLoader.PhysicalNode previouslyHitNode;
        private float nodeHitGlowStrength;

        public EdgeGun(GraphLoader graphLoader, Material edgeMaterial)
        {
            this.graphLoader = graphLoader;
            this.edgeMaterial = edgeMaterial;
        }

        public string GetGunName() => "Edge";

        public void OnMoveDown(Transform playerTransform, Camera camera)
        {
            var transform = camera.transform;
            bool hit = Physics.Raycast(
                transform.position,
                transform.forward,
                out var hitInfo,
                hitDistance);
            if (!hit) return;
            var gameObjectHit = hitInfo.collider.gameObject;
            if (IsNotPhysicalNode(gameObjectHit)) return;
            if (HitTheSamePhysicalNode(gameObjectHit)) return;

            var currentlyHitPhysicalNode = graphLoader.FindPhysicalNodeByGameObject(gameObjectHit);

            if (previouslyHitNode == null)
            {
                previouslyHitNode = currentlyHitPhysicalNode;
                previouslyHitNode.physicalNode.EnableGlow();
                nodeHitGlowStrength = 0.5f;
                previouslyHitNode.physicalNode.SetGlow(nodeHitGlowStrength);
                return;
            }

            if (EdgeOfNodesAlreadyExists(previouslyHitNode, currentlyHitPhysicalNode)) return;

            var physicalEdge = CreatePhysicalEdge(currentlyHitPhysicalNode);
            graphLoader.physicalEdges.Add(physicalEdge);
            previouslyHitNode.physicalNode.DisableGlow();
            previouslyHitNode = null;
        }

        private GraphLoader.PhysicalEdge CreatePhysicalEdge(GraphLoader.PhysicalNode currentlyHitPhysicalNode)
        {
            var edgeObject = EdgeGenerator.CreateGameObjectEdge(
                previouslyHitNode.node,
                currentlyHitPhysicalNode.node,
                edgeMaterial
            );

            var physicalEdge = new GraphLoader.PhysicalEdge
            {
                edge = edgeObject,
                nodeFrom = previouslyHitNode,
                nodeTo = currentlyHitPhysicalNode
            };
            return physicalEdge;
        }

        private bool EdgeOfNodesAlreadyExists(GraphLoader.PhysicalNode first, GraphLoader.PhysicalNode second)
        {
            var alreadyExistingEdge = graphLoader
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
            return !gameObjectHit.CompareTag(Tags.PhysicalNodeTag);
        }

        public void OnSwitchedAway()
        {
            // noop
        }
    }
}