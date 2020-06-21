using UnityEngine;

namespace Assets.Scripts
{
  public class PlayerMovement : MonoBehaviour
  {
    SettingsManager _settingsManager;
    public CharacterController controller;
    public float speed = 12f;

    public void Start() => _settingsManager = new SettingsManager();

    public void Update()
    {
      if (GameService.Instance.IsPaused)
        return;

      switch (_settingsManager.PlayerMovementMode)
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

    void ApplyMovementAxisBased()
    {
      var x = Input.GetAxis("Horizontal");
      var y = Input.GetAxis("Up");
      var z = Input.GetAxis("Vertical");

      var offset = (transform.right * x) + (transform.up * y) + (transform.forward * z);
      controller.Move(offset * speed * Time.deltaTime);
    }

    void ApplyMovementFollowCamera()
    {
      var x = Input.GetAxis("Horizontal");
      var y = Input.GetAxis("Up");
      var z = Input.GetAxis("Vertical");

      var offset = (transform.right * x) + (transform.up * y) + (Camera.main.transform.forward * z);
      controller.Move(offset * speed * Time.deltaTime);
    }
  }
}
