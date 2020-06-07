using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static GraphLoader;

namespace Assets.Canvas.PropertyContainer
{
    public class APointContainer : MonoBehaviour
    {
        private PhysicalNode PhysicalNode;
        public InputField ax;
        public InputField ay;
        public InputField az;
        public InputField alpha_theta;
        public InputField alpha_phi;
        public InputField alpha_psi;

        public void SetPhysicalNode(PhysicalNode physicalNode)
        {
            PhysicalNode = physicalNode;

            ax.text = PhysicalNode.node.apoint.ax.ToString();
            ay.text = PhysicalNode.node.apoint.ay.ToString();
            az.text = PhysicalNode.node.apoint.az.ToString();
            alpha_theta.text = PhysicalNode.node.apoint.alpha_theta.ToString();
            alpha_phi.text = PhysicalNode.node.apoint.alpha_phi.ToString();
            alpha_psi.text = PhysicalNode.node.apoint.alpha_psi.ToString();
        }

        public void SaveData()
        {

        }
    }
}
