using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //Script to move the camera as the player
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed at which the camera 
    // moves


    [Header("Camera Settings")]
    public float mouseSensitivity = 2f; // Sensitivity of mouse movement
    public Transform player; // Reference to the player transform


    [Header("Walking Animation")]
    public float walkingFrequency = 14f;
    public float walkingAmplitude = 0.05f;

    [Range(-1f, 1f)]
    public float stepTriggerThreshold = -0.4f;

    private float walkingTimer = 0f; // Timer to track walking animation
    private float defaultCameraYPosition;
    private float verticalRotation = 0f; // Current vertical rotation of the camera


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor

        defaultCameraYPosition = player.localPosition.y; // Store the default Y position of the camera
    }
    void Update()
    {
        float currentMoveX = Input.GetAxis("Horizontal");
        float currentMoveZ = Input.GetAxis("Vertical");

        HandleMouseLook();
        HandleMovement(currentMoveX, currentMoveZ);
        WalkingAnimation(currentMoveX, currentMoveZ);
    }

    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the camera horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Limit vertical rotation
        player.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleMovement(float moveX, float moveZ)
    {
        // Calculate movement direction
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        transform.position += moveDirection * moveSpeed * Time.deltaTime; // Move the camera
    }

    private void WalkingAnimation(float moveX, float moveZ)
    {
        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
        {
            walkingTimer += Time.deltaTime * walkingFrequency; // Increment the walking timer based on frequency

            float currentSinValue = Mathf.Sin(walkingTimer);

            float newYPosition = defaultCameraYPosition + Mathf.Sin(walkingTimer) * walkingAmplitude; // Calculate new Y position using sine wave
            player.localPosition = new Vector3(player.localPosition.x, newYPosition, player.localPosition.z); // Update camera's Y position
        }
        else
        {
            walkingTimer = 0f; // Reset the walking timer when not moving
            float smoothYReturn = Mathf.Lerp(player.localPosition.y, defaultCameraYPosition, Time.deltaTime * walkingFrequency); // Smoothly return to default Y position
            player.localPosition = new Vector3(player.localPosition.x, smoothYReturn, player.localPosition.z); // Update camera's Y position
        }
    }
}
