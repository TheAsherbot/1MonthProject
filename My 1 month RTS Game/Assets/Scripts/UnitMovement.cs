using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class UnitMovement : MonoBehaviour
{


    [SerializeField] private float reachedWayPointDistance = 0.05f;

    
    private bool isMoving;
    
    private List<Vector2> movementPath;
    private Grid grid;


    [Header("Movement")]
    [SerializeField] private float timeToMove = 2;
    [SerializeField] private AnimationCurve movement_Curve;

    private float movement_ElapsedTime;
    private Vector3 movement_StartPosition;
    private Vector2 movement_EndPosition;


    [Header("Rotation")]
    [SerializeField] private float timeToRotate = 0.5f;
    [SerializeField] private AnimationCurve rotation_Curve;

    private float rotation_ElapsedTime;
    private Vector3 rotation_StartPosition;
    private Vector2 rotation_EndPosition;

    
    private void Awake()
    {
        movementPath = new List<Vector2>();
    }

    private void Start()
    {
        grid = GridManager.Instance.Grid;
        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Update()
    {
        TestInput();

        Rotate();

        Move();
    }


    private void TestInput()
    {
        if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.LeftControl))
        {
            MovementInputPresed();
        }
    }

    private void MovementInputPresed()
    {
        if (isMoving)
        {
            movementPath = grid.FindPathAsVector2s(movement_EndPosition, Mouse2D.GetMousePosition2D());
        }
        else
        {
            movementPath = grid.FindPathAsVector2s(transform.position, Mouse2D.GetMousePosition2D());
        }
        isMoving = true;
        DrawPathfingLines();
    }

    private void Rotate()
    {
        rotation_ElapsedTime += Time.deltaTime;
        float percentageComplate = rotation_ElapsedTime / timeToRotate;

        transform.up = Vector3.Slerp(rotation_StartPosition, rotation_EndPosition, rotation_Curve.Evaluate(percentageComplate));
    }

    private void Move()
    {
        movement_ElapsedTime += Time.deltaTime;
        float percentageComplate = movement_ElapsedTime / timeToMove;

        transform.position = Vector3.Lerp(movement_StartPosition, movement_EndPosition, movement_Curve.Evaluate(percentageComplate));

        if (Mathf.Abs(Vector2.Distance(movement_EndPosition, transform.position)) <= reachedWayPointDistance)
        {
            movement_ElapsedTime = 0;
            if (movementPath != null)
            {
                if (movementPath.Count > 1)
                {
                    SubpathReached();
                }
                else
                {
                    PathReached();
                }
            }
        }
    }

    private void SubpathReached()
    {
        movementPath.RemoveAt(0);
        movement_EndPosition = movementPath[0];

        movement_StartPosition = transform.position;
        
        rotation_ElapsedTime = 0;
        rotation_StartPosition = transform.up;
        rotation_EndPosition = movement_EndPosition - (Vector2)transform.position;
    }

    private void PathReached()
    {
        movementPath.Clear();
        movement_EndPosition = transform.position;
        movement_StartPosition = transform.position;

        isMoving = false;
    }

    private void Grid_OnGridValueChanged(object sender, Grid.OnGridValueChangedEventArgs e)
    {
        foreach (Vector2 node in movementPath)
        {
            grid.GetXY(node, out int x, out int y);
            if (x == e.x && y == e.y)
            {
                if (node == movementPath[movementPath.Count - 1])
                {
                    movementPath.Remove(node);
                    return;
                }
                movementPath = grid.FindPathAsVector2s(movement_EndPosition, movementPath[movementPath.Count - 1]);
            }
        }
    }

    private void DrawPathfingLines()
    {
        if (movementPath != null)
        {
            for (int i = 0; i < movementPath.Count - 1; i++)
            {
                Debug.DrawLine(movementPath[i], movementPath[i + 1], Color.green, 5f);
            }
        }
    }


}
