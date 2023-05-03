using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{


    [SerializeField] private float movementSpeed = 15;


    private Vector2 moveDirrection;


    private void Update()
    {
        moveDirrection = Vector2.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            // Up
            moveDirrection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Down
            moveDirrection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // Left
            moveDirrection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Right
            moveDirrection += Vector2.right;
        }

        transform.position += new Vector3(moveDirrection.x, moveDirrection.y) * Time.deltaTime * movementSpeed;
    }

}
