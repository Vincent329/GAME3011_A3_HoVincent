using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public bool elementIsActive;

    PlayerControlInput playerActions;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerActions = InputManager.inputActions;
        Debug.Log("Initiate Grid");
        elementIsActive = true;
        playerActions.Minigame.ReturnToPlayer.performed += OnReturnToPlayer;
        playerActions.Minigame.ReturnToPlayer.canceled += OnReturnToPlayer;
        
    }

    private void OnEnable()
    {
        if (elementIsActive)
        {
            playerActions.Minigame.ReturnToPlayer.performed += OnReturnToPlayer;
            playerActions.Minigame.ReturnToPlayer.canceled += OnReturnToPlayer;
        }
    }

    private void OnDisable()
    {
        playerActions.Minigame.ReturnToPlayer.performed -= OnReturnToPlayer;
        playerActions.Minigame.ReturnToPlayer.canceled -= OnReturnToPlayer;

    }

    private void OnReturnToPlayer(InputAction.CallbackContext obj)
    {
        Debug.Log("Return To Player"); 
        GameManager.Instance.TogglePanel();

        InputManager.ToggleActionMap(playerActions.Player);
    }
}
