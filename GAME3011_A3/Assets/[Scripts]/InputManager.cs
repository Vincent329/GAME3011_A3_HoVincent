using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{

    public static PlayerControlInput inputActions;
    public static event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        inputActions = new PlayerControlInput();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        ToggleActionMap(inputActions.Player);
    }

    public static void ToggleActionMap(InputActionMap inputMap)
    {
        Debug.Log("Switch to " + inputMap.ToString());
        if (inputMap.enabled)
            return;

        inputActions.Disable();
        actionMapChange?.Invoke(inputMap);
        inputMap.Enable();
    }
}
