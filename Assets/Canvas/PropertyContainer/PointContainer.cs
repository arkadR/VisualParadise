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
    public class PointContainer : MonoBehaviour
    {
        private PhysicalNode PhysicalNode;
        public InputField x;
        public InputField y;
        public InputField z;
        public InputField theta;
        public InputField phi;
        public InputField psi;

        public void SetPhysicalNode(PhysicalNode physicalNode)
        {
            PhysicalNode = physicalNode;

            x.text = PhysicalNode.node.point.x.ToString();
            y.text = PhysicalNode.node.point.y.ToString();
            z.text = PhysicalNode.node.point.z.ToString();
            theta.text = PhysicalNode.node.point.theta.ToString();
            phi.text = PhysicalNode.node.point.phi.ToString();
            psi.text = PhysicalNode.node.point.psi.ToString();
        }

        public void SaveData()
        {
            
        }
    }
}
