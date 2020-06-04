using UnityEngine;

namespace Player.Guns
{
    public class NodeGun : IGun
    {
        private readonly GraphLoader graphLoader;
        private readonly Material nodeMaterial;

        public NodeGun(GraphLoader graphLoader, Material nodeMaterial)
        {
            this.graphLoader = graphLoader;
            this.nodeMaterial = nodeMaterial;
        }

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

        public void OnSwitchedAway()
        {
            //noop
        }
    }
}