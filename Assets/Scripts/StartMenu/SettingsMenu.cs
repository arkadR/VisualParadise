using Assets.Scripts.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.StartMenu
{
  public class SettingsMenu : MonoBehaviour
  {
    const string c_playerMovement = "Player movement";

    SettingsManager _settingsManager;

    public Slider mouseSensitivitySlider;

    public Text playerMovementText;

    void Start()
    {
      _settingsManager = new SettingsManager();
      playerMovementText.text =
        $"{c_playerMovement}: {EnumUtils<PlayerMovementMode>.GetName(_settingsManager.PlayerMovementMode)}";
      mouseSensitivitySlider.value = _settingsManager.MouseSensitivity;
      mouseSensitivitySlider.onValueChanged.AddListener(MouseSensitivity_OnSlide);
    }

    public void PlayerMovement_OnClick()
    {
      var nextPlayerMovementMode = _settingsManager.ChangeToNextPlayerMovementMode();
      playerMovementText.text =
        $"{c_playerMovement}: {EnumUtils<PlayerMovementMode>.GetName(nextPlayerMovementMode)}";
    }

    void MouseSensitivity_OnSlide(float sliderValue) => _settingsManager.MouseSensitivity = sliderValue;
  }
}
