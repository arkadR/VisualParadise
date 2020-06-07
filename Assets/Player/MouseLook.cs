using UnityEngine;

public class MouseLook : MonoBehaviour
{
  public float mouseSensitivity = 100f;

  public Transform playerBody;

  public float yRotation = 0f;

  // Start is called before the first frame update
  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
  }

  // Update is called once per frame
  void Update()
  {
    if (GameService.Instance.IsPaused)
      return;
    
    var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    yRotation -= mouseY;
    yRotation = Mathf.Clamp(yRotation, -90f, 90f);

    transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    playerBody.Rotate(Vector3.up * mouseX);
  }
}
