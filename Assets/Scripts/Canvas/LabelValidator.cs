using UnityEngine.UI;

namespace Assets.Scripts.Canvas
{
  public class LabelValidator
  {
    private InputField _inputField;

    public LabelValidator(InputField inputField)
    {
      _inputField = inputField;
    }

    public void OnValueChanged()
    {
      if (_inputField.text.Length < 1)
        _inputField.GetComponent<Image>().color = new UnityEngine.Color(1, 0.7f, 0.7f);
      else
        _inputField.GetComponent<Image>().color = new UnityEngine.Color(1, 1, 1);
    }
  }
}
