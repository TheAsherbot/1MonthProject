using System;
using System.Collections.Generic;

using TheAshBot;
using TheAshBot.TwoDimentional;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Civilian : _BaseUnit, ISelectable, IDamageable, IMoveable
{

    public enum States
    {
        Idle,
        Moving,
        MovingToMine,
        Mining,
        ReturnFromMining,
    }


    #region Variables

    [Header("Generial")]
    [SerializeField] private bool isOnPlayerTeam;
    [SerializeField] private GameObject selectedVisual;
    [field: SerializeField]
    public List<HotBarSlotSO> HotBarSlotSOList
    {
        get;
        set;
    }


    public bool IsSelected
    {
        get;
        set;
    }
    private States state;


    [Header("Mining")]
    [SerializeField] private float timeToMine = 3f;
    [SerializeField] private TownHall townHall;

    private bool hasMinerials;
    private bool isReturningFromMiningSate;
    private Vector3 minerial;


    private HealthSystem healthSystem;


    private GameInputActions inputActions;


    #endregion


    #region Unity Functions

    private void Start()
    {
        Vector2 healthBarOffset = Vector3.up * 2;
        Vector2 healthBarSize = new Vector3(2, 0.3f);
        healthSystem = HealthBar.Create(10, transform, healthBarOffset, healthBarSize, Color.red, Color.gray, new HealthBar.Border { color = Color.black, thickness = 0.1f }, false, 13);

        inputActions = new GameInputActions();
        inputActions.Game.Enable();

        healthSystem.OnHealthDepleted += HealthSystem_OnHealthDepleted;
        OnReachedDestination += Civilian_OnReachedDestination;
        OnStopMoveing += Civilian_OnReachedDestination;
    }


    private void Update()
    {
        TestState();
    }

    #endregion


    #region Input

    private Vector2 TestIfShouldMine(Vector2 position)
    {
        if (GridManager.Instance == null) return Vector2.zero;
        
        if (hasMinerials)
        {
            Trigger_OnMove(townHall.GetLoadTransform().position);
        }

        GridObject gridObject = GridManager.Instance.grid.GetGridObject(position);
        if (gridObject.tilemapSprite != GridObject.TilemapSprite.Minerials)
        {
            state = States.Moving;
            return position;
        }

        minerial = GridManager.Instance.grid.GetWorldPosition(gridObject.X, gridObject.Y);

        GridObject closestNeighbour = GetClosedtNeighbourFromMinerial(gridObject);
        state = States.MovingToMine;
        Debug.Log(closestNeighbour);
        return GridManager.Instance.grid.GetWorldPosition(closestNeighbour.X, closestNeighbour.Y);
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
        if (hasMinerials)
        {
            state = States.ReturnFromMining;
        }

        if (GridManager.Instance.grid.GetGridObject(minerial).neighbourNodeList.Contains(GridManager.Instance.grid.GetGridObject(transform.position)))
        {
            Trigger_OnStopMoveing();
            state = States.Mining;
        }
    }

    private async void MiningState()
    {
        GridManager.Instance.grid.GetGridObject(transform.position).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.NotWalkable });
        await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(1000 * timeToMine));
        if (state == States.Mining)
        {
            state = States.ReturnFromMining;
        }
    }

    private void ReturnFromMiningState()
    {
        if (!isReturningFromMiningSate)
        {
            hasMinerials = true;
            isReturningFromMiningSate = true;
            Trigger_OnMove(townHall.GetLoadTransform().position);
        }
    }

    #endregion


    #region Events

    private void HealthSystem_OnHealthDepleted(object sender, EventArgs e)
    {
        TeamManager teamManager = isOnPlayerTeam ? TeamManager.PlayerInstance : TeamManager.AIInstance;
        teamManager.UnitKilled(this);
        Destroy(gameObject);
    }

    private void Civilian_OnReachedDestination(object sender, EventArgs e)
    {
        if (state == States.ReturnFromMining)
        {
            townHall.AddMinerials();
            hasMinerials = false;

            GridObject closestNeighbour = GetClosedtNeighbourFromMinerial(GridManager.Instance.grid.GetGridObject(minerial));
            this.Log(GridManager.Instance);
            Trigger_OnMove(GridManager.Instance.grid.GetWorldPosition(closestNeighbour.X, closestNeighbour.Y));
            state = States.MovingToMine;
            isReturningFromMiningSate = false;
            return;
        }

        state = States.Idle;
    }

    #endregion


    #region Interfaces

    #region ISelectable

    public void Select()
    {
        IsSelected = true;

        selectedVisual.SetActive(true);
    }

    public void Unselect()
    {
        IsSelected = false;

        selectedVisual.SetActive(false);
    }

    public bool UsesHotbar()
    {
        return false;
    }

    public void OnSlot1ButtonClicked()
    {
    }

    public void OnSlot2ButtonClicked()
    {
    }

    public void OnSlot3ButtonClicked()
    {
    }

    #endregion

    #region IDamagable

    public void Damage(int amount)
    {
        healthSystem.Damage(amount);
    }

    #endregion

    #region IMoveable

    public void Move(Vector2 position)
    {
        if (IsSelected)
        {
            if (!GridManager.Instance.grid.IsPositionOnGrid(position)) return;

            Trigger_OnMove(TestIfShouldMine(position));
        }
    }

    public void Move(float x, float y)
    {
        Move(new Vector2(x, y));
    }

    #endregion


    #endregion


    #region Other

    public void SetTownHall(TownHall townHall) 
    {
        this.townHall = townHall;
    }

    
    private GridObject GetClosedtNeighbourFromMinerial(GridObject minerialGridObject)
    {
        GridObject closestNeighbour = null;

        return minerialGridObject;
        /*
        foreach (GridObject neighbour in minerialGridObject.neighbourNodeList)
        {
            if (neighbour.StateList.Contains(GridObject.OccupationState.NotWalkable)) continue;
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
        */
        return closestNeighbour;
    }

    #endregion


}
