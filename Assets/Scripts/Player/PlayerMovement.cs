using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float gravity = -9.81f;

    [Header("Cámara")]
    public float mouseSensitivity = 2f;
    public float verticalClamp = 80f;
    public Transform cameraPlayer;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool lockCursor;

    [Header("Interacción")]
    public float interactDistance = 4f;
    public Image target;

    private IInteractable currentInteractable;
    private IInteractable heldInteractable;

    [Header("Detección pared invisible")]
    public bool isInsideInvisibleWall = false;

    private void Start()
    {
        target.color = Color.white;
        TryGetComponent(out controller);
        lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Movimiento
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        // Gravedad
        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Cámara
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);
        cameraPlayer.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Interacción
        DetectInteractable();
    }

    public void OnLockMouse(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            lockCursor = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            lockCursor = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnGrabbed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isInsideInvisibleWall)
        {
            Debug.Log("No puedo llevar eso.");
            return;
        }

        if (heldInteractable != null)
        {
            heldInteractable.Interact(); // soltar
            heldInteractable = null;
            return;
        }

        if (currentInteractable != null)
        {
            // Solo cajas con tag permitido
            GameObject obj = (currentInteractable as MonoBehaviour).gameObject;
            if (obj.CompareTag("CarryableBox"))
            {
                heldInteractable = currentInteractable;
                heldInteractable.Interact(); // agarrar
            }
        }
    }

    private void DetectInteractable()
    {
        if (heldInteractable != null)
        {
            target.color = Color.white;
            currentInteractable = null;
            return;
        }

        Ray ray = new Ray(cameraPlayer.position, cameraPlayer.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                if (currentInteractable == interactable) return;
                currentInteractable = interactable;
                target.color = Color.yellow;
                return;
            }
        }

        currentInteractable = null;
        target.color = Color.white;
    }

    // Detectar si el jugador entra o sale de una pared invisible
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WallForBox"))
            isInsideInvisibleWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WallForBox"))
            isInsideInvisibleWall = false;
    }
}
