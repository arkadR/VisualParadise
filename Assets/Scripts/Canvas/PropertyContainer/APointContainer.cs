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

    public void Start()
    {
      ax.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(ax); });
      ay.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(ay); });
      az.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(az); });
      alpha_theta.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(alpha_theta); });
      alpha_phi.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(alpha_phi); });
      alpha_psi.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(alpha_psi); });
    }

    public void SetNode(Node node)
    {
      _node = node;

      (ax.text, ay.text, az.text) = node.Acceleration.ToStringTuple();
      (alpha_theta.text, alpha_phi.text, alpha_psi.text) = node.AngularAcceleration.ToStringTuple();
    }

    public bool IsInputCorrect()
    {
      return float.TryParse(ax.text, out var ax_res) && float.TryParse(ay.text, out var ay_res)
        && float.TryParse(az.text, out var az_res) && float.TryParse(alpha_theta.text, out var alpha_theta_res)
        && float.TryParse(alpha_phi.text, out var alpha_phi_res) && float.TryParse(alpha_psi.text, out var alpha_psi_res);
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
