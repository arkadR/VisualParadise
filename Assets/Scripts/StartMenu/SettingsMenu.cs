using Assets.Scripts.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.StartMenu
{
  public class SettingsMenu : MonoBehaviour
  {
    const string c_playerMovement = "Player movement";

    public Text playerMovementText;

    public SettingsManager settingsManager;

    void Start()
    {
      settingsManager = FindObjectOfType<SettingsManager>();
      playerMovementText.text =
        $"{c_playerMovement}: {EnumUtils<PlayerMovementMode>.GetName(settingsManager.PlayerMovementMode)}";
    }

    public void PlayerMovement_OnClick()
    {
      var nextPlayerMovementMode = settingsManager.ChangeToNextPlayerMovementMode();
      playerMovementText.text =
        $"{c_playerMovement}: {EnumUtils<PlayerMovementMode>.GetName(nextPlayerMovementMode)}";
    }
  }
}
