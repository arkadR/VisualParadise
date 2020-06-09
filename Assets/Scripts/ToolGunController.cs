using UnityEngine;

public class ToolGunController : MonoBehaviour
{
  public TextMesh gunModeText;

  public void SetGunModeText(string text)
  {
    gunModeText.text = text;
  }
}
