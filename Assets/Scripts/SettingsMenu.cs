using Assets.Scripts;
using Assets.Scripts.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
  private const string c_playerMovement = "Player movement";

  public Text playerMovementText;

  private int PlayerMovementModeValue
  {
    get => PlayerPrefs.GetInt(Constants.PlayerMovementModeKey);
    set => PlayerPrefs.SetInt(Constants.PlayerMovementModeKey, value);
  }

  void Start() => SetPlayerMovementText(EnumUtils<PlayerMovementMode>.DefinedOrDefaultCast(PlayerMovementModeValue));

  public void PlayerMovement_OnClick()
  {
    PlayerMovementModeValue = EnumUtils<PlayerMovementMode>.GetNextValue(PlayerMovementModeValue);
    Debug.Log("PlayerMovementModeKey: " + PlayerMovementModeValue);
    SetPlayerMovementText(EnumUtils<PlayerMovementMode>.DefinedOrDefaultCast(PlayerMovementModeValue));
  }

  private void SetPlayerMovementText(PlayerMovementMode value)
  {
    playerMovementText.text =
      $"{c_playerMovement}: {EnumUtils<PlayerMovementMode>.GetName(value)}";
  }
}
