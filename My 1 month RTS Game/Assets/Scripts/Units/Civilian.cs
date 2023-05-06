using System;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class Civilian : _BaseUnit, ISelectable
{

    public enum States
    {
        Idle,
        Moving,
        MovingToMine,
        Mining,
        ReturnFromMining,
    }


    [field : Header("Generial")]
    public bool IsSelected
    {
        get;
        set;
    }
    private States state;


    [Header("Mining")]
    [SerializeField] private float gatherMinerialRange = 1.5f;
    [SerializeField] private float timeToMine = 3f;

    private TownHall townHall;


    private HealthSystem healthSystem;

    private Vector2 movePoint;


    #region Unity Functions

    private void Start()
    {
        healthSystem = HealthBar.Create(10, transform, Vector3.up, new Vector3(2, 0.3f), Color.red, Color.gray, new HealthBar.Border { color = Color.black, thickness = 0.1f });
        
        OnMove += Civilian_OnMove;
        OnReachedDestination += Civilian_OnReachedDestination;
        OnStopMoveing += Civilian_OnReachedDestination;
    }
    
    private void Update()
    {
        if (IsSelected)
        {
            TestInput();

            TestState();
        }
    }

    #endregion


    public void SetTownHall(TownHall townHall) 
    {
        this.townHall = townHall;
    }


    #region Input

    private void TestInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MoveInputPressed();
        }
    }

    private void MoveInputPressed()
    {
        GridObject gridObject = GridManager.Instance.grid.GetGridObject(Mouse2D.GetMousePosition2D());
        if (gridObject.tilemapSprite != GridObject.TilemapSprite.Minerials)
        {
            state = States.Moving;
            Trigger_OnMove(Mouse2D.GetMousePosition2D());
            return;
        }

        GridObject closestNeighbour = null;
        foreach (GridObject neighbour in gridObject.neighbourNodeList)
        {
            if (neighbour.State != GridObject.OccupationState.Empty) continue;
            if (closestNeighbour == null)
            {
                closestNeighbour = neighbour;
                continue;
            }

            if (Vector2.Distance(transform.position, GridManager.Instance.grid.GetWorldPosition(neighbour.X, neighbour.Y)) <
                Vector2.Distance(transform.position, GridManager.Instance.grid.GetWorldPosition(closestNeighbour.X, closestNeighbour.Y)))
            {
                // this node in closer.
                closestNeighbour = neighbour;
            }
        }

        Trigger_OnMove(GridManager.Instance.grid.GetWorldPosition(closestNeighbour.X, closestNeighbour.Y));
        state = States.MovingToMine;
    }

    #endregion


    #region State

    private void TestState()
    {
        switch (state)
        {
            case States.Idle:

                break;
            case States.Moving:
                
                break;
            case States.MovingToMine:
                MoveingToMineState();
                break;
            case States.Mining:
                MiningState();
                break;
            case States.ReturnFromMining:
                ReturnFromMiningState();
                break;
            default:
                state = States.Idle;
                break;
        }
    }

    private void MoveingToMineState()
    {
        if (movePoint != null && Vector2.Distance(transform.position, movePoint) < gatherMinerialRange)
        {
            Trigger_OnStopMoveing();
            state = States.Mining;
        }
    }

    private async void MiningState()
    {
        await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(1000 * timeToMine));
        if (state == States.Mining)
        {
            state = States.ReturnFromMining;
        }
    }

    private void ReturnFromMiningState()
    {
        Trigger_OnMove(townHall.GetLoadTransform().position);
    }

    #endregion


    #region Events

    private void Civilian_OnMove(object sender, OnMoveEventArgs e)
    {
        movePoint = e.movePoint;
    }

    private void Civilian_OnReachedDestination(object sender, EventArgs e)
    {
        movePoint = default;
        state = States.Idle;
    }

    #endregion


    #region Interfaces

    public void Select()
    {
        IsSelected = true;
    }

    public void Unselect()
    {
        IsSelected = false;
    }

    #endregion


}
