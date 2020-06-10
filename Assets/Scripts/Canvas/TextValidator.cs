using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Canvas
{
  public class TextValidator
  {
    private InputField _inputField;

    public TextValidator(InputField inputField)
    {
      _inputField = inputField;
    }

    public void OnValueChanged()
    {
      if (!float.TryParse(_inputField.text, out float input))
        _inputField.GetComponent<Image>().color = new UnityEngine.Color(1, 0.7f, 0.7f);
      else
        _inputField.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1);
    }
  }
}
