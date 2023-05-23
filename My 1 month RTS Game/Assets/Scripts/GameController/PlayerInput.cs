using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameRTSController))]
public class PlayerInput : MonoBehaviour
{


    private GameRTSController controller;
    private GameInputActions inputActions;


    private void Start()
    {
        GetComponent<GameRTSController>();
        inputActions = new GameInputActions();
        inputActions.Game.Enable();
        inputActions.Game.Select.started += Select_started;
        inputActions.Game.Select.canceled += Select_canceled;
        inputActions.Game.Action1.canceled += Action1_canceled;
    }

    private void Action1_canceled(InputAction.CallbackContext obj)
    {

    }

    private void Select_canceled(InputAction.CallbackContext obj)
    {
        
    }

    private void Select_started(InputAction.CallbackContext obj)
    {
        
    }
}
