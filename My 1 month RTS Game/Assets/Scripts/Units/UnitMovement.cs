using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using Unity.VisualScripting;

using UnityEngine;

public class UnitMovement : MonoBehaviour
{


    [Header("Movement")]
    [SerializeField] private float reachedWayPointDistance = 0.05f;
    [SerializeField] private float timeToMove = 2;
    [SerializeField] private AnimationCurve movement_Curve;

    private float movement_ElapsedTime;
    private Vector2 movement_StartPosition;
    private Vector2 movement_EndPosition;

    private Vector2 lastMouseUpPosition;


    [Header("Rotation")]
    [SerializeField] private float timeToRotate = 0.5f;
    [SerializeField] private AnimationCurve rotation_Curve;

    private float rotation_ElapsedTime;
    private Vector2 rotation_StartPosition;
    private Vector2 rotation_EndPosition;


    private Grid grid;
    private List<Vector2> movementPath;

    

    private void Awake()
    {
        movementPath = new List<Vector2>();
    }

    private void Start()
    {
        grid = GridManager.Instance.Grid;
    }

    private void Update()
    {
        TestInput();

        Rotate();

        Move();
    }


    private void TestInput()
    {
        if (Input.GetMouseButtonUp(1) && !Input.GetKey(KeyCode.LeftControl))
        {
            lastMouseUpPosition = Mouse2D.GetMousePosition2D();
            FindPath();
        }
    }

    private void FindPath()
    {
        Vector2 endPosition = lastMouseUpPosition;
        List <Vector2> lastMovementPath;


        FindPath();

        while (movementPath == null)
        {
            movementPath = lastMovementPath;
            movementPath.RemoveAt(movementPath.Count - 1);
            endPosition = movementPath[movementPath.Count - 1];
            FindPath();
        }
        DrawPathfingLines();

        void FindPath()
        {
            lastMovementPath = movementPath;
            movementPath = grid.FindPathAsVector2s(transform.position, endPosition, new List<GridObject.OccupationState>
            {
                GridObject.OccupationState.NotWalkable,
            });
            movementPath = grid.FindPathAsVector2s(transform.position, endPosition, new List<GridObject.OccupationState>
            {
                GridObject.OccupationState.NotWalkable,
            });
        }
    }

    private void Rotate()
    {
        rotation_ElapsedTime += Time.deltaTime;
        float percentageComplate = rotation_ElapsedTime / timeToRotate;

        transform.up = Vector3.Slerp(rotation_StartPosition, rotation_EndPosition, rotation_Curve.Evaluate(percentageComplate));
    }

    private void Move()
    {
        if (movementPath == null || movementPath.Count == 0) return;

        if (movementPath[0] == null) return;

        movement_ElapsedTime += Time.deltaTime;
        float percentageComplate = movement_ElapsedTime / timeToMove;

        transform.position = Vector3.Lerp(movement_StartPosition, movement_EndPosition, movement_Curve.Evaluate(percentageComplate));

        if (Mathf.Abs(Vector2.Distance(movement_EndPosition, transform.position)) <= reachedWayPointDistance)
        {
            movement_ElapsedTime = 0;
            SubpathReached();
        }
    }

    private void SubpathReached()
    {
        if (movementPath.Count == 2)
        {
            // Has reached Destination
            PathReached();
            return;
        }

        FindPath();

        movement_StartPosition = transform.position;
        movement_EndPosition = movementPath[1];


        rotation_ElapsedTime = 0;
        rotation_StartPosition = transform.up;
        rotation_EndPosition = movement_EndPosition - (Vector2)transform.position;
    }

    private void PathReached()
    {
        movementPath.Clear();
        movement_EndPosition = transform.position;
        movement_StartPosition = transform.position;
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
