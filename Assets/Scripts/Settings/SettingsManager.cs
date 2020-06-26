using Assets.Scripts.Common;
using Assets.Scripts.Common.Utils;
using UnityEngine;

namespace Assets.Scripts.Settings
{
  public class SettingsManager
  {
    public float MouseSensitivity
    {
      get => PlayerPrefs.GetFloat(Constants.MouseSensitivityKey, Constants.DefaultMouseSensitivity);
      set => PlayerPrefs.SetFloat(Constants.MouseSensitivityKey, value);
    }

    public float MovementSpeed
    {
      get => PlayerPrefs.GetFloat(Constants.MovementSpeedKey, Constants.DefaultMovementSpeed);
      set => PlayerPrefs.SetFloat(Constants.MovementSpeedKey, value);
    }

    public PlayerMovementMode PlayerMovementMode =>
      EnumUtils<PlayerMovementMode>.DefinedOrDefaultCast(PlayerPrefs.GetInt(Constants.PlayerMovementModeKey));


    public PlayerMovementMode ChangeToNextPlayerMovementMode()
    {
      var playerMovementKey = PlayerPrefs.GetInt(Constants.PlayerMovementModeKey);
      var next = EnumUtils<PlayerMovementMode>.GetNextValue(playerMovementKey);
      PlayerPrefs.SetInt(Constants.PlayerMovementModeKey, next);
      return PlayerMovementMode;
    }
  }
}
