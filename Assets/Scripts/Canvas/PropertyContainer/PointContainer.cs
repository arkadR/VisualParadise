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

    public void SetNode(Node node)
    {
      _node = node;
      
      (x.text, y.text, z.text) = _node.Position.ToStringTuple();
      (theta.text, phi.text, psi.text) = _node.Position.ToStringTuple();
    }

    public void SaveData()
    {

    }
  }
}
