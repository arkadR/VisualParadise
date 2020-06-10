using Assets.Scripts.Common.Extensions;
using UnityEngine;

namespace Assets.Scripts
{
  public class PlayerMovement : MonoBehaviour
  {
    private PlayerMovementMode _movementMode;
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float speed = 12f;

    private void Start()
    {
      var playerMovementModeValue = PlayerPrefs.GetInt(Constants.PlayerMovementMode);
      _movementMode = EnumUtils<PlayerMovementMode>.DefinedOrDefaultCast(playerMovementModeValue);
    }

    private void Update()
    {
      if (GameService.Instance.IsPaused)
      {
        return;
      }

      switch (_movementMode)
      {
        case PlayerMovementMode.AxisBased:
        {
          ApplyMovementAxisBased();
          break;
        }
        case PlayerMovementMode.FollowCamera:
        {
          ApplyMovementFollowCamera();
          break;
        }
      }
    }

    private void ApplyMovementAxisBased()
    {
      var x = Input.GetAxis("Horizontal");
      var z = Input.GetAxis("Vertical");
      var y = Input.GetAxis("Up");

      var offset = (transform.right * x) + (transform.forward * z) + (transform.up * y);

      controller.Move(offset * speed * Time.deltaTime);
    }

    private void ApplyMovementFollowCamera()
    {
      var z = Input.GetAxis("Vertical");

      controller.Move(Camera.main.transform.forward * speed * Time.deltaTime * z);
    }
  }
}
