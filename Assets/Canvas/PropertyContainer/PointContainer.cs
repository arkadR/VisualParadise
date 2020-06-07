using Assets.GameObject;
using Assets.Model;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Canvas.PropertyContainer
{
    public class PointContainer : MonoBehaviour
    {
        private Node _node;
        public InputField x;
        public InputField y;
        public InputField z;
        public InputField theta;
        public InputField phi;
        public InputField psi;

        public void SetNode(Node node)
        {
            _node = node;

            var (posX, posY, posZ) = _node.Position;
            var (rotX, rotY, rotZ) = _node.Rotation;

            x.text = posX.ToString();
            y.text = posY.ToString();
            z.text = posZ.ToString();
            theta.text = rotX.ToString();
            phi.text = rotY.ToString();
            psi.text = rotZ.ToString();
        }

        public void SaveData()
        {
            
        }
    }
}
