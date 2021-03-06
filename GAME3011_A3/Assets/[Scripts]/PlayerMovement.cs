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
            GameManager.Instance.StartAtDifficulty += StartMinigame;
        }
    }

    private void OnDisable()
    {
        playerActions.Player.Movement.performed -= OnMovement;
        playerActions.Player.Movement.canceled -= OnMovement;
        GameManager.Instance.StartAtDifficulty -= StartMinigame;


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

        GameManager.Instance.StartAtDifficulty += StartMinigame;
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

    private void StartMinigame()
    {
        GameManager.Instance.TogglePanel();
        playerVelocity = Vector3.zero; // zero out the velocity of the player
        InputManager.ToggleActionMap(playerActions.Minigame);
    }
    
}
