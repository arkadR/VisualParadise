using System.Collections.Generic;
using UnityEngine;
using static GraphLoader;

namespace Player.Guns
{
    public class EdgeGun : IGun
    {
        private float hitDistance = 20f;
        private readonly GraphLoader graphLoader;
        private readonly Material edgeMaterial;
        private GraphLoader.PhysicalNode previouslyHitNode;

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
                SetGlow(previouslyHitNode, true);
                return;
            }

            if (EdgeOfNodesAlreadyExists(previouslyHitNode, currentlyHitPhysicalNode)) return;

            var physicalEdge = CreatePhysicalEdge(currentlyHitPhysicalNode);
            graphLoader.physicalEdges.Add(physicalEdge);
            SetGlow(previouslyHitNode, false);
            previouslyHitNode = null;
        }

        void SetGlow(GraphLoader.PhysicalNode physicalNode, bool value)
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

        public void OnRightClick(Camera camera)
        {
            
        }
    }
}