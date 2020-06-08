using UnityEngine;

namespace Assets.Scripts
{
  public class PlayerMovement : MonoBehaviour
  {
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float speed = 12f;

    // Update is called once per frame
    void Update()
    {
      if (GameService.Instance.IsPaused)
        return;

      ApplyPlayerInput();
    }

    void ApplyPlayerInput()
    {
      var x = Input.GetAxis("Horizontal");
      var z = Input.GetAxis("Vertical");
      var y = Input.GetAxis("Up");

      var offset = transform.right * x + transform.forward * z + transform.up * y;

      controller.Move(offset * speed * Time.deltaTime);
    }
  }
}
