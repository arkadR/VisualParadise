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

    }
  }
}
