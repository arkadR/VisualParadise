using UnityEngine;

namespace Player.Guns
{
    public class NodeGun : IGun
    {
        private readonly Material nodeMaterial;

        public NodeGun(Material nodeMaterial)
        {
            this.nodeMaterial = nodeMaterial;
        }

        public void OnMoveDown(Transform playerTransform)
        {
            NodeGenerator.GenerateNode(
                playerTransform.position,
                playerTransform.rotation,
                nodeMaterial);
        }

        public void OnSwitchedAway()
        {
            //noop
        }
    }
}