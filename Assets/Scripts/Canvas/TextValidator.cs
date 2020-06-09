using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas
{
  public static class TextValidator
  {
    public static void OnValueChanged(InputField inputField)
    {
      if (!float.TryParse(inputField.text, out float input))
        inputField.GetComponent<Image>().color = new UnityEngine.Color(1, 0.7f, 0.7f);
      else
        inputField.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1);
    }
  }
}
