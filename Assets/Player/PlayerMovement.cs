using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public CharacterController controller;
  public Transform groundCheck;
  public LayerMask groundMask;

  public float speed = 12f;
  public float gravity = -9.81f;
  public float groundCheckDistance = 0.4f;
  public float jumpHeight = 2f;

  private Vector2 _velocity;
  private bool _isGrounded;

  // Update is called once per frame
  void Update()
  {
    ApplyPlayerInput();
    ApplyGravity();
    HandleJump();
  }

  void ApplyPlayerInput()
  {
    var x = Input.GetAxis("Horizontal");
    var z = Input.GetAxis("Vertical");

    var offset = transform.right * x + transform.forward * z;

    controller.Move(offset * speed * Time.deltaTime);
  }

  void HandleJump()
  {
    if (Input.GetButtonDown("Jump") && _isGrounded)
    {
      _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
  }

  void ApplyGravity()
  {
    _isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
    if (_isGrounded && _velocity.y < 0f)
      _velocity.y = -2f;

    _velocity.y += gravity * Time.deltaTime;
    controller.Move(_velocity * Time.deltaTime);
  }
}
