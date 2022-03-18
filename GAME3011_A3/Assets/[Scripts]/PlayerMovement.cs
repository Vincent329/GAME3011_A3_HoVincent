using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerControlInput playerActions;

    private void OnEnable()
    {

        playerActions.Player.Movement.performed += OnMovement;
        playerActions.Player.Movement.canceled += OnMovement;
    }

    private void OnDisable()
    {
        playerActions.Player.Movement.performed -= OnMovement;
        playerActions.Player.Movement.canceled -= OnMovement;
    }
    // Start is called before the first frame update
    void Awake()
    {
        playerActions = new PlayerControlInput();
        Debug.Log("Initiate");
        playerActions.Player.Enable();
        playerActions.Player.Movement.performed += OnMovement;
        playerActions.Player.Movement.canceled += OnMovement;

    }
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ReadValue<Vector2>());
    }

}
