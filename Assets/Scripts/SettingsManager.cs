using Assets.Scripts.Common;
using Assets.Scripts.Common.Utils;
using UnityEngine;

namespace Assets.Scripts
{
  public class SettingsManager
  {
    public float MouseSensitivity
    {
      get => PlayerPrefs.GetFloat(Constants.MouseSensitivityKey, 100f);
      set => PlayerPrefs.SetFloat(Constants.MouseSensitivityKey, value);
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
