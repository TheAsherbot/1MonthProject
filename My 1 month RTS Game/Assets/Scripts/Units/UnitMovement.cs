using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using Unity.VisualScripting;

using UnityEditor.Rendering;

using UnityEngine;

[RequireComponent(typeof(_BaseUnit))]
public class UnitMovement : MonoBehaviour
{

    #region Variables

    [Header("References")]
    private Grid grid;
    private _BaseUnit unit;


    [Header("Movement")]
    [SerializeField] private float reachedWayPointDistance = 0.05f;
    [SerializeField] private float timeToMove = 2;
    [SerializeField] private AnimationCurve movement_Curve;

    private float movement_ElapsedTime;
    private Vector2 movement_StartPosition;
    private Vector2 movement_EndPosition;
    private List<Vector2> movementPath;

    private Vector2 lastMouseUpPosition;


    [Header("Rotation")]
    [SerializeField] private float timeToRotate = 0.5f;
    [SerializeField] private AnimationCurve rotation_Curve;

    private float rotation_ElapsedTime;
    private Vector2 rotation_StartPosition;
    private Vector2 rotation_EndPosition;

    #endregion


    #region MonoBehaviour Functions

    private void Awake()
    {
        Debug.Log("Awake");
        movementPath = new List<Vector2>();

        unit = GetComponent<Unit>();
        unit.OnMoveInputPressed += Unit_OnMoveInputPressed;
    }

    private void Start()
    {
        grid = GridManager.Instance.Grid;
        movement_StartPosition = transform.position;
    }

    private void Update()
    {
        Rotate();

        Move();
    }

    #endregion


    #region Private Functions

    private void Unit_OnMoveInputPressed(object sender, _BaseUnit.OnMoveInputPressedEventArgs e)
    {
        lastMouseUpPosition = e.mousePosition;
        StartPath();
    }

    private void FindPath()
    {
        Vector2 endPosition = lastMouseUpPosition;
        List<Vector2> lastMovementPath;

        FindPath();

        while (movementPath == null)
        {
            movementPath = lastMovementPath;
            if (movementPath.Count > 1)
            {
                movementPath.RemoveAt(movementPath.Count - 1);
                endPosition = movementPath[movementPath.Count - 1];
                FindPath();
            }
        }
        DrawPathfingLines();

        void FindPath()
        {
            lastMovementPath = movementPath;
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
        if (transform.position == Vector3.zero)
        {
            Debug.Log("movement_StartPosition: " + movement_StartPosition);
            Debug.Log("movement_EndPosition: " + movement_EndPosition);
            Debug.Log("Movement");
        }

        if (Mathf.Abs(Vector2.Distance(movement_EndPosition, transform.position)) <= reachedWayPointDistance)
        {
            movement_ElapsedTime = 0;
            SubpathReached();
        }
    }

    private void StartPath()
    {
        FindPath();

        movement_StartPosition = transform.position;
        if (movementPath.Count > 1)
        {
            movement_EndPosition = movementPath[1];
        }
        else
        {
            movement_EndPosition = movementPath[0];
        }

        rotation_ElapsedTime = 0;
        rotation_StartPosition = transform.up;
        rotation_EndPosition = movement_EndPosition - (Vector2)transform.position;
    }

    private void SubpathReached()
    {
        if (movementPath.Count == 1)
        {
            // Has reached Destination
            PathReached();
            return;
        }

        FindPath();

        movement_StartPosition = transform.position;
        if (movementPath.Count > 1)
        {
            movement_EndPosition = movementPath[1];
        }
        else
        {
            movement_EndPosition = movementPath[0];
        }

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

    #endregion

}
