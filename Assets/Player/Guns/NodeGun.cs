using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GraphLoader;

namespace Player.Guns
{
    public class NodeGun : MonoBehaviour, IGun
    {
        private readonly GraphLoader graphLoader;
        private readonly Material nodeMaterial;

        public NodeGun(GraphLoader graphLoader, Material nodeMaterial)
        {
            this.graphLoader = graphLoader;
            this.nodeMaterial = nodeMaterial;
        }

        public string GetGunName() => "Node";

        public void OnMoveDown(Transform playerTransform, Camera camera)
        {
            var transform = camera.transform;
            var forward = transform.forward;
            var sphere = NodeGenerator.GeneratePhysicalNode(
                transform.position + forward * 3,
                transform.rotation,
                nodeMaterial);
            var position = sphere.transform.position;
            var id = graphLoader.physicalNodes.Count;
            var node = GraphLoader.Node.ZeroNode(id);
            node.point.SetPosition(position);
            graphLoader.physicalNodes.Add(
                new GraphLoader.PhysicalNode
                {
                    id = id,
                    node = node,
                    physicalNode = sphere
                }
            );
        }

        public void OnRightClick(Camera camera)
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                var node = hit.collider.gameObject;
                var contextMenuHandler = FindObjectOfType<ContextMenuHandler>();
                contextMenuHandler.SetPhysicalNode(graphLoader.physicalNodes, graphLoader.physicalEdges, node);
            }
        }

        public void OnSwitchedAway()
        {
            //noop
        }
    }
}