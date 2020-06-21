using Assets.Scripts.Common;
using Assets.Scripts.Common.Utils;
using UnityEngine;

namespace Assets.Scripts
{
  public class SettingsManager : MonoBehaviour
  {
    int _playerMovementModeValue;

    public PlayerMovementMode PlayerMovementMode =>
      EnumUtils<PlayerMovementMode>.DefinedOrDefaultCast(_playerMovementModeValue);

    public void Start() => _playerMovementModeValue = PlayerPrefs.GetInt(Constants.PlayerMovementModeKey);

    public PlayerMovementMode ChangeToNextPlayerMovementMode()
    {
      var next = EnumUtils<PlayerMovementMode>.GetNextValue(_playerMovementModeValue);
      _playerMovementModeValue = next;
      PlayerPrefs.SetInt(Constants.PlayerMovementModeKey, _playerMovementModeValue);
      return PlayerMovementMode;
    }
  }
}
