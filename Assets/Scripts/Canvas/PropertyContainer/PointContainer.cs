using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas.PropertyContainer
{
  public class PointContainer : MonoBehaviour
  {
    Node _node;
    public GraphService graphService;
    public InputField phi;
    public InputField psi;
    public InputField theta;
    public InputField x;
    public InputField y;
    public InputField z;

    public void Start()
    {
      x.onValueChanged.AddListener(delegate { new TextValidator(x).OnValueChanged(); });
      y.onValueChanged.AddListener(delegate { new TextValidator(y).OnValueChanged(); });
      z.onValueChanged.AddListener(delegate { new TextValidator(z).OnValueChanged(); });
      theta.onValueChanged.AddListener(delegate { new TextValidator(theta).OnValueChanged(); });
      phi.onValueChanged.AddListener(delegate { new TextValidator(phi).OnValueChanged(); });
      psi.onValueChanged.AddListener(delegate { new TextValidator(psi).OnValueChanged(); });
    }

    public void SetNode(Node node)
    {
      _node = node;

      (x.text, y.text, z.text) = _node.Position.ToStringTuple();
      (theta.text, phi.text, psi.text) = _node.Rotation.ToStringTuple();
    }

    public bool IsInputCorrect() =>
      float.TryParse(x.text, out _)
      && float.TryParse(y.text, out _)
      && float.TryParse(z.text, out _)
      && float.TryParse(theta.text, out _)
      && float.TryParse(phi.text, out _)
      && float.TryParse(psi.text, out _);

    public void SaveData()
    {
      var x_value = float.Parse(x.text);
      var y_value = float.Parse(y.text);
      var z_value = float.Parse(z.text);
      var theta_value = float.Parse(theta.text);
      var phi_value = float.Parse(phi.text);
      var psi_value = float.Parse(psi.text);

      var newPosition = new Vector3(x_value, y_value, z_value);
      var newRotation = new Vector3(theta_value, phi_value, psi_value);
      _node.Position = newPosition;
      _node.Rotation = newRotation;
      graphService.FixEdgesOfNode(_node);
    }
  }
}
