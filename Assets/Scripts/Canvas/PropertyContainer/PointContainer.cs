using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas.PropertyContainer
{
  public class PointContainer : MonoBehaviour
  {
    private Node _node;
    public InputField x;
    public InputField y;
    public InputField z;
    public InputField theta;
    public InputField phi;
    public InputField psi;

    public void Start()
    {
      x.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(x); });
      y.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(y); });
      z.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(z); });
      theta.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(theta); });
      phi.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(phi); });
      psi.onValueChanged.AddListener(delegate { TextValidator.OnValueChanged(psi); });
    }

    public void SetNode(Node node)
    {
      _node = node;
      
      (x.text, y.text, z.text) = _node.Position.ToStringTuple();
      (theta.text, phi.text, psi.text) = _node.Rotation.ToStringTuple();
    }

    public bool IsInputCorrect()
    {
      return float.TryParse(x.text, out var x_res) && float.TryParse(y.text, out var y_res)
        && float.TryParse(z.text, out var z_res) && float.TryParse(theta.text, out var theta_res)
        && float.TryParse(phi.text, out var phi_res) && float.TryParse(psi.text, out var psi_res);
    }

    public void SaveData()
    {
      float x_value = float.Parse(x.text);
      float y_value = float.Parse(y.text);
      float z_value = float.Parse(z.text);
      float theta_value = float.Parse(theta.text);
      float phi_value = float.Parse(phi.text);
      float psi_value = float.Parse(psi.text);

      var newPosition = new Vector3(x_value, y_value, z_value);
      var newRotation = new Vector3(theta_value, phi_value, psi_value);
      _node.Position = newPosition;
      _node.Rotation = newRotation;

      var graphService = FindObjectOfType<GraphService>();
      var edges = graphService.FindNodeEdges(_node);

      foreach (var edge in edges)
      {
        graphService.FixEdge(edge);
      }
    }
  }
}
