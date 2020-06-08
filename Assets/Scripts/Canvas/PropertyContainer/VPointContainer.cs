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
      float vx_value = float.Parse(vx.text);
      float vy_value = float.Parse(vy.text);
      float vz_value = float.Parse(vz.text);
      float om_theta_value = float.Parse(om_theta.text);
      float om_phi_value = float.Parse(om_phi.text);
      float om_psi_value = float.Parse(om_psi.text);

      var newVelocity = new Vector3(vx_value, vy_value, vz_value);
      var newAngularVelocity = new Vector3(om_theta_value, om_phi_value, om_psi_value);
      _node.Velocity = newVelocity;
      _node.AngularVelocity = newAngularVelocity;
    }
  }
}
