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
            var sphere = NodeGenerator.GenerateNode(
                transform.position + forward * 3,
                transform.rotation,
                nodeMaterial);
            var position = sphere.transform.position;
            var node = new GraphLoader.Node
            {
                apoint = null,
                id = 0,
                point = new GraphLoader.Point
                {
                    x = position.x,
                    y = position.y,
                    z = position.z
                },
                vpoint = null
            };
            graphLoader.physicalNodes.Add(
                new GraphLoader.PhysicalNode
                {
                    id = 0,
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