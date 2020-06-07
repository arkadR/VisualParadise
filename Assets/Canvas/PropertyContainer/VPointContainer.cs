using Assets.GameObject;
using Assets.Model;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Canvas.PropertyContainer
{
    public class VPointContainer : MonoBehaviour
    {
        private Node _node;
        public InputField vx;
        public InputField vy;
        public InputField vz;
        public InputField om_theta;
        public InputField om_phi;
        public InputField om_psi;

        public void SetNode(Node node)
        {
            _node = node;
            var (velX, velY, velZ) = _node.Velocity;
            var (velAngX, velAngY, velAngZ) = _node.Velocity;

            vx.text = velX.ToString();
            vy.text = velY.ToString();
            vz.text = velZ.ToString();
            om_theta.text = velAngX.ToString();
            om_phi.text = velAngY.ToString();
            om_psi.text = velAngZ.ToString();
        }

        public void SaveData()
        {
            
        }
    }
}
