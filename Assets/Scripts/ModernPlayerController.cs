using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ModernPlayerController : MonoBehaviour
{
    [Header("New Input System")]
    public InputAction moveAction;
    public InputAction lookAction;

    [Header("Movimiento y Cámara")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;
    public Transform playerCamera;
    public float mouseSensitivity = 0.2f;

    [Header("Audio Pasos (Bloques C y F)")]
    public AudioSource sourcePasos;
    public AudioClip[] clipsDePasos;
    public float tiempoEntrePasos = 0.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private float temporizadorPasos;

    // El New Input System requiere activar las acciones por código
    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. ROTACIÓN DE CÁMARA
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        xRotation -= lookInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

        // 2. MOVIMIENTO FÍSICO
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * walkSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 3. LA LÓGICA DE AUDIO (isMoving)
        bool isMoving = moveInput.magnitude > 0.1f;

        if (isMoving)
        {
            temporizadorPasos += Time.deltaTime;

            if (temporizadorPasos >= tiempoEntrePasos)
            {
                ReproducirPaso();
                temporizadorPasos = 0f;
            }
        }
        else
        {
            // Resetea el tiempo para que el primer paso suene apenas toques WASD
            temporizadorPasos = tiempoEntrePasos;
        }
    }

    private void ReproducirPaso()
    {
        if (clipsDePasos.Length == 0 || sourcePasos == null) return;

        sourcePasos.clip = clipsDePasos[Random.Range(0, clipsDePasos.Length)];
        sourcePasos.pitch = Random.Range(0.9f, 1.1f);
        sourcePasos.volume = Random.Range(0.3f, 0.5f);
        sourcePasos.Play();
    }
}
