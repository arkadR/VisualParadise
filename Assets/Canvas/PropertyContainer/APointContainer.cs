using Assets.GameObject;
using Assets.Model;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Canvas.PropertyContainer
{
    public class APointContainer : MonoBehaviour
    {
        private Node _node;
        public InputField ax;
        public InputField ay;
        public InputField az;
        public InputField alpha_theta;
        public InputField alpha_phi;
        public InputField alpha_psi;

        public void SetNode(Node node)
        {
            _node = node;
            var (nodeAx, nodeAy, nodeAz) = _node.Acceleration;
            var (nodeAax, nodeAay, nodeAaz) = _node.AngularAcceleration;

            ax.text = nodeAx.ToString();
            ay.text = nodeAy.ToString();
            az.text = nodeAz.ToString();
            alpha_theta.text = nodeAax.ToString();
            alpha_phi.text = nodeAay.ToString();
            alpha_psi.text = nodeAaz.ToString();
        }

        public void SaveData()
        {

        }
    }
}
