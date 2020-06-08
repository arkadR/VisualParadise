using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas.PropertyContainer
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

      (ax.text, ay.text, az.text) = node.Acceleration.ToStringTuple();
      (alpha_theta.text, alpha_phi.text, alpha_psi.text) = node.AngularAcceleration.ToStringTuple();
    }

    public void SaveData()
    {
      float ax_value = float.Parse(ax.text);
      float ay_value = float.Parse(ay.text);
      float az_value = float.Parse(az.text);
      float alpha_theta_value = float.Parse(alpha_theta.text);
      float alpha_phi_value = float.Parse(alpha_phi.text);
      float alpha_psi_value = float.Parse(alpha_psi.text);

      var newAcceleration = new Vector3(ax_value, ay_value, az_value);
      var newAngularAcceleration = new Vector3(alpha_theta_value, alpha_phi_value, alpha_psi_value);
      _node.Acceleration = newAcceleration;
      _node.AngularAcceleration = newAngularAcceleration;
    }
  }
}
