using Assets.Scripts.Common.Utils;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
  public class SettingsMenuBinding : MonoBehaviour
  {
    const string c_playerMovement = "Player movement";

    SettingsManager _settingsManager;

    public GameObject mouseSensitivity;

    public GameObject movementSpeed;

    public GameObject playerMovement;

    public void Start()
    {
      _settingsManager = new SettingsManager();
      BindPlayerMovementToggleButton();
      BindMouseSensitivitySlider();
      BindMovementSpeedSlider();
    }

    void BindPlayerMovementToggleButton()
    {
      var playerMovementText = playerMovement.GetComponentInChildren<Text>();
      playerMovementText.text =
        $"{c_playerMovement}: {EnumUtils<PlayerMovementMode>.GetName(_settingsManager.PlayerMovementMode)}";
      playerMovement.GetComponent<Button>().onClick.AddListener(() =>
      {
        var nextPlayerMovementMode = _settingsManager.ChangeToNextPlayerMovementMode();
        playerMovementText.text =
          $"{c_playerMovement}: {EnumUtils<PlayerMovementMode>.GetName(nextPlayerMovementMode)}";
      });
    }

    void BindMouseSensitivitySlider()
    {
      var mouseSensitivitySlider = mouseSensitivity.GetComponentInChildren<Slider>();
      mouseSensitivitySlider.value = _settingsManager.MouseSensitivity;
      mouseSensitivitySlider.onValueChanged.AddListener(sliderValue => _settingsManager.MouseSensitivity = sliderValue);
    }

    void BindMovementSpeedSlider()
    {
      var movementSpeedSlider = movementSpeed.GetComponentInChildren<Slider>();
      movementSpeedSlider.value = _settingsManager.MovementSpeed;
      movementSpeedSlider.onValueChanged.AddListener(sliderValue => _settingsManager.MovementSpeed = sliderValue);
    }
  }
}
