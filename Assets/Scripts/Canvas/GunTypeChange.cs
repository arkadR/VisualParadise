using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas
{
  public class GunTypeChange : MonoBehaviour
  {
    public Text gunType;

    public void ChangeGunType(string value)
    {
      gunType.text = value;
    }
  }
}
