using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{


    [SerializeField] private float movementSpeed = 15;
    [SerializeField] private Vector2 bottomLeftBounds = new Vector2(0, 0);
    [SerializeField] private Vector2 topRightBounds = new Vector2(100, 100);


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

    private void LateUpdate()
    {
        if (transform.position.x > topRightBounds.x)
        {
            transform.position = new Vector2(topRightBounds.x, transform.position.y);
        }
        if (transform.position.y > topRightBounds.y)
        {
            transform.position = new Vector2(transform.position.x, topRightBounds.y);
        }
        if (transform.position.y < bottomLeftBounds.y)
        {
            transform.position = new Vector2(transform.position.x, bottomLeftBounds.y);
        }
        if (transform.position.x < bottomLeftBounds.x)
        {
            transform.position = new Vector2(bottomLeftBounds.x, transform.position.y);
        }
    }

}
