using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Look")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Bloquea el cursor en el centro de la pantalla y lo oculta
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotación vertical (mirar arriba/abajo) aplicada a la cámara
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal (mirar a los lados) aplicada a todo el jugador
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Movimiento local respecto hacia donde mira el jugador
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * walkSpeed * Time.deltaTime);

        // Aplicar gravedad
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Fuerza mínima para mantenerlo pegado al piso
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}