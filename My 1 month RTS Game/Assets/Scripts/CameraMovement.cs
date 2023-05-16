using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{


    [SerializeField] private float movementSpeed = 15;


    private Vector2 moveDirrection;
    private GameInputActions inputActions;


    private void Start()
    {
        inputActions = new GameInputActions();
        inputActions.CameraMovement.Enable();
    }

    private void Update()
    {
        moveDirrection = inputActions.CameraMovement.Movement.ReadValue<Vector2>();
        
        transform.position += new Vector3(moveDirrection.x, moveDirrection.y) * Time.deltaTime * movementSpeed;
    }

}
