using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private Vector3 playerVelocity;
    private Rigidbody rb;

    [Header("Minigame Entry Variables")]
    private bool inRange;
    public bool elementIsActive;
    public bool activeGame;

    PlayerControlInput playerActions;
    private PlayerInput playerInput;

    private void OnEnable()
    {
        if (elementIsActive)
        {
            playerActions.Player.Movement.performed += OnMovement;
            playerActions.Player.Movement.canceled += OnMovement;
            playerActions.Player.Interact.started += OnInteract;
        }
    }

    private void OnDisable()
    {
        playerActions.Player.Movement.performed -= OnMovement;
        playerActions.Player.Movement.canceled -= OnMovement;
        playerActions.Player.Interact.started -= OnInteract;

    }
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerActions = InputManager.inputActions;
        Debug.Log("Initiate");
        elementIsActive = true;
        playerActions.Player.Movement.performed += OnMovement;
        playerActions.Player.Movement.canceled += OnMovement;
        playerActions.Player.Interact.started += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(rb.position + playerVelocity * playerSpeed * Time.deltaTime);
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        Vector2 movementVector = obj.ReadValue<Vector2>();
        playerVelocity = new Vector3(movementVector.x, 0, movementVector.y);
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("Switching to Match 3");
        GameManager.Instance.TogglePanel();
        InputManager.ToggleActionMap(playerActions.Minigame);
    }
}
