using UnityEngine;

namespace Player.Guns
{
    public class NodeGun : MonoBehaviour, IGun
    {
        private GraphLoader graphLoader;
        private Material nodeMaterial;

        private void Start()
        {
            nodeMaterial = Resources.Load<Material>("Materials/Node Material");
            graphLoader = FindObjectOfType<GraphLoader>();
        }

        public NodeGun(GraphLoader graphLoader, Material nodeMaterial)
        {
            this.graphLoader = graphLoader;
            this.nodeMaterial = nodeMaterial;
        }

        public string GetGunName() => "Node";

        public void OnMoveDown(Transform playerTransform, Camera camera)
        {
            var cameraTransform = camera.transform;
            var forward = cameraTransform.forward;
            var sphere = NodeGenerator.GeneratePhysicalNode(
                cameraTransform.position + forward * 3,
                cameraTransform.rotation,
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