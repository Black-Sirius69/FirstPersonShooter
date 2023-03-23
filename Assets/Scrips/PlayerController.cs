using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera = null;

    [Range(1f, 10f)] public float sensitivity = 10f;
    public float walkSpeed = 10f;
    float yRotation;
    public float stopTime = 0.3f;
    public float gravity = -9.8f;
    Vector2 currentDirection = Vector2.zero;
    Vector2 currentVelocity = Vector2.zero;
    public float jumpForce = 5f;
    float yVelocity;
    CharacterController playerController;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        OnMouseLook();
        OnPlayerMove();
    }

    void OnMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up * mouseDelta.x * sensitivity);

        yRotation -= mouseDelta.y * sensitivity;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * yRotation;
    }

    void OnPlayerMove()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDirection.Normalize();

        currentDirection = Vector2.SmoothDamp(currentDirection, inputDirection, ref currentVelocity, stopTime);

        if (playerController.isGrounded)
            yVelocity = 0;

        if (playerController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            yVelocity = Mathf.Sqrt(-2 * gravity * jumpForce);
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * walkSpeed + Vector3.up * yVelocity;
        playerController.Move(velocity * Time.deltaTime);
    }
}
