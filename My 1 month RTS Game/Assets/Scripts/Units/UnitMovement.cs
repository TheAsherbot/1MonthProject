using System.Collections.Generic;

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

    private bool stopedMoving;
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
        movementPath = new List<Vector2>();

        unit = GetComponent<Civilian>();
    }

    private void Start()
    {
        grid = GridManager.Instance.grid;
        movement_StartPosition = transform.position;


        unit.OnMove += Unit_OnMove;
        unit.OnStopMoveing += Unit_OnStopMoveing;
    }

    private void Update()
    {
        Rotate();

        Move();
    }

    #endregion


    #region Private Functions

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

        DrawPath();

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
        

        if (Mathf.Abs(Vector2.Distance(movement_EndPosition, transform.position)) <= reachedWayPointDistance)
        {
            movement_ElapsedTime = 0;
            SubpathReached();
        }
    }

    private void StartPath()
    {
        FindPath();


        GridManager.Instance.grid.GetGridObject(movement_EndPosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.Empty });

        movement_StartPosition = transform.position;
        if (movementPath.Count > 1)
        {
            movement_EndPosition = movementPath[1];
        }
        else if (movementPath.Count == 1)
        {
            movement_EndPosition = movementPath[0];
        }
        else
        {
            Debug.Log("movementPath.Count == 0");
        }

        GridManager.Instance.grid.GetGridObject(movement_EndPosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.NotPlaceable });

        rotation_ElapsedTime = 0;
        rotation_StartPosition = transform.up;
        rotation_EndPosition = movement_EndPosition - (Vector2)transform.position;
    }

    private void SubpathReached()
    {

        if (stopedMoving)
        {
            PathStoped();
            return;
        }
        if (movementPath.Count == 1)
        {
            PathReached();
            return;
        }

        FindPath();

        GridManager.Instance.grid.GetGridObject(movement_EndPosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.Empty });

        movement_StartPosition = transform.position;
        if (movementPath.Count > 1)
        {
            movement_EndPosition = movementPath[1];
        }
        else
        {
            movement_EndPosition = movementPath[0];
        }

        GridManager.Instance.grid.GetGridObject(movement_EndPosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.NotPlaceable});

        rotation_ElapsedTime = 0;
        rotation_StartPosition = transform.up;
        rotation_EndPosition = movement_EndPosition - (Vector2)transform.position;
    }

    private void PathReached()
    {
        movementPath.Clear();
        movement_EndPosition = transform.position;
        movement_StartPosition = transform.position;

        unit.Trigger_OnReachedDestination();
    }

    private void PathStoped()
    {
        movementPath.Clear();
        movement_EndPosition = transform.position;
        movement_StartPosition = transform.position;

        stopedMoving = false;
    }

    #endregion


    #region Events

    private void Unit_OnStopMoveing(object sender, System.EventArgs e)
    {
        stopedMoving = true;
    }

    private void Unit_OnMove(object sender, _BaseUnit.OnMoveEventArgs e)
    {
        lastMouseUpPosition = e.movePoint;
        StartPath();
    }

    #endregion



    private void DrawPath()
    {
        for (int i = 1; i < movementPath.Count; i++)
        {
            Debug.DrawLine(movementPath[i -1], movementPath[i], Color.green, 5f);
        }
    }



}
