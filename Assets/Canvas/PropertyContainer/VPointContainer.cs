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
    public class VPointContainer : MonoBehaviour
    {
        private PhysicalNode PhysicalNode;
        public InputField vx;
        public InputField vy;
        public InputField vz;
        public InputField om_theta;
        public InputField om_phi;
        public InputField om_psi;

        public void SetPhysicalNode(PhysicalNode physicalNode)
        {
            PhysicalNode = physicalNode;

            vx.text = PhysicalNode.node.vpoint.vx.ToString();
            vy.text = PhysicalNode.node.vpoint.vy.ToString();
            vz.text = PhysicalNode.node.vpoint.vz.ToString();
            om_theta.text = PhysicalNode.node.vpoint.om_theta.ToString();
            om_phi.text = PhysicalNode.node.vpoint.om_phi.ToString();
            om_psi.text = PhysicalNode.node.vpoint.om_psi.ToString();
        }

        public void SaveData()
        {
            
        }
    }
}
