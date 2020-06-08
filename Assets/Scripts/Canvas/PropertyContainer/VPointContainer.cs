using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas.PropertyContainer
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
      
      (vx.text, vy.text, vz.text) = _node.Velocity.ToStringTuple();
      (om_theta.text, om_phi.text, om_psi.text) = _node.AngularVelocity.ToStringTuple();
    }

    public void SaveData()
    {

    }
  }
}
