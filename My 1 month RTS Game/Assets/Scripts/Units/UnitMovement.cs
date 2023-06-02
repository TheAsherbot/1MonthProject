using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Pathfinding;

using TheAshBot;

using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(_BaseUnit))]
public class UnitMovement : MonoBehaviour
{

    #region Events

    public event EventHandler<OnDirectionChangedEventArgs> OnDirectionChanged;
    public class OnDirectionChangedEventArgs : EventArgs
    {
        public Vector2 direction;
    }

    #endregion


    #region Variables

    [Header("References")]
    private Grid grid;
    private _BaseUnit unit;


    [Header("Movement")]
    [SerializeField] private float reachedWayPointDistance = 0.05f;
    [SerializeField] private float timeToMove = 0.3f;
    [SerializeField] private AnimationCurve movement_Curve;

    private bool stopedMoving;
    private bool isMoving;
    private float movement_ElapsedTime;
    private Vector2 movement_StartPosition;
    private Vector2 movement_EndPosition;
    private Vector2 lastMoveDirection;
    private Vector2 movePoint;

    #endregion

    private Seeker seeker;
    private Path path;


    #region MonoBehaviour Functions

    private void Awake()
    {
        unit = GetComponent<_BaseUnit>();
        seeker = GetComponent<Seeker>();
    }

    private void Start()
    {
        grid = GridManager.Instance.grid;
        movement_StartPosition = transform.position;


        unit.OnMove += Unit_OnMove;
        Debug.Log("unit.OnMove += Unit_OnMove");
        unit.OnStopMoveing += Unit_OnStopMoveing;
    }

    private void Update()
    {
        Move();
    }

    #endregion


    #region Path Finding

    private void StartPath()
    {
        this.Log("StartPath");

        float cellSize = GridManager.Instance.grid.GetCellSize();
        movePoint = Vector2Int.FloorToInt(movePoint) + new Vector2(cellSize / 2, cellSize / 2);

        if (isMoving == true)
        {
            return;
        }
        else
        {
            FindPath();
        }
    }

    private void FindPath()
    {
        Vector2 endPosition = movePoint;

        FindPath();

        void FindPath()
        {
            seeker.StartPath(transform.position, endPosition, OnPathCalculated);
        }
    }

    private void OnPathCalculated(Path path)
    {
        if (path.error)
        {
            this.LogError("There was a error when calculating the path\n" + path.errorLog);
            return;
        }

        this.path = path;

        if (path.vectorPath.Count >= 2)
        {
            if ((Vector2)(path.vectorPath[1] - path.vectorPath[0]) != lastMoveDirection)
            {
                lastMoveDirection = path.vectorPath[1] - path.vectorPath[0];
                InvokeOnDirectionChanged();
            }
        }
        else
        {
            lastMoveDirection = Vector2.zero;
            InvokeOnDirectionChanged();
        }

        isMoving = true;

        GridManager.Instance.grid.GetGridObject(movement_EndPosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.Empty });

        if (!TrySetEndAndStartPositions())
        {
            return;
        }

        GridManager.Instance.grid.GetGridObject(movement_EndPosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.NotPlaceable });

    }

    private void SubpathReached()
    {
        if (stopedMoving)
        {
            PathStoped();
            return;
        }

        if (IsPathNull()) return;

        if (path.vectorPath.Count <= 1 || MathF.Abs(Vector2.Distance(transform.position, movePoint)) <= reachedWayPointDistance)
        {
            PathReached();
            return;
        }

        path.vectorPath.RemoveAt(0);

        GridManager.Instance.grid.GetGridObject(movement_EndPosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.Empty });
        
        if (!TrySetEndAndStartPositions())
        {
            return;
        }

        GridManager.Instance.grid.GetGridObject(movement_EndPosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.NotPlaceable});
    }

    private bool TrySetEndAndStartPositions()
    {
        if (IsPathNull())
        {
            return false;
        }

        movement_StartPosition = transform.position;
        if (path.vectorPath.Count > 1)
        {
            movement_EndPosition = path.vectorPath[1];
        }
        else if (path.vectorPath.Count == 1)
        {
            movement_EndPosition = path.vectorPath[0];
        }
        else
        {
            PathReached();
            return false;
        }
        return true;
    }

    private bool IsPathNull()
    {
        if (path == null) return true;

        if (path.vectorPath == null) return true;

        if (path.vectorPath.Count == 0) return true;

        if (path.vectorPath[0] == null) return true;

        return false;
    }

    private void PathReached()
    {
        path.vectorPath.Clear();
        movement_EndPosition = transform.position;
        movement_StartPosition = transform.position;
        isMoving = false;

        unit.Trigger_OnReachedDestination();
    }

    private void PathStoped()
    {
        path.vectorPath.Clear();
        movement_EndPosition = transform.position;
        movement_StartPosition = transform.position;

        stopedMoving = false;
    }

    #endregion

    private void Move()
    {
        if (IsPathNull())
        {
            return;
        }

        movement_ElapsedTime += Time.deltaTime;
        float percentageComplate = movement_ElapsedTime / timeToMove;


        transform.position = Vector3.Lerp(movement_StartPosition, movement_EndPosition, movement_Curve.Evaluate(percentageComplate));
        

        if (Mathf.Abs(Vector2.Distance(movement_EndPosition, transform.position)) <= reachedWayPointDistance)
        {
            movement_ElapsedTime = 0;
            if (MathF.Abs(Vector2.Distance(transform.position, movePoint)) <= reachedWayPointDistance)
            {
                PathReached();
                return;
            }
            SubpathReached();
        }
    }

    private void InvokeOnDirectionChanged()
    {
        OnDirectionChanged?.Invoke(this, new OnDirectionChangedEventArgs
        {
            direction = lastMoveDirection,
        });
    }


    #region Events

    private void Unit_OnStopMoveing(object sender, System.EventArgs e)
    {
        stopedMoving = true;
        isMoving = false;
        
        lastMoveDirection = Vector2.zero;
        InvokeOnDirectionChanged();
    }

    private void Unit_OnMove(object sender, _BaseUnit.OnMoveEventArgs e)
    {
        this.Log("Unit_OnMove");
        movePoint = e.movePoint;
        StartPath();
    }

    #endregion

}
