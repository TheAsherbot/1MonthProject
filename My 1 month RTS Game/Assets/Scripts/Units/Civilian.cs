using System;
using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;
using UnityEngine.InputSystem;

public class Civilian : _BaseUnit, ISelectable, IDamageable
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

    private bool isReturningFromMiningSate;
    private Vector3 minerial;


    private HealthSystem healthSystem;


    private GameInputActions inputActions;


    #endregion


    #region Unity Functions

    private new void Start()
    {
        base.Start();

        Vector2 healthBarOffset = Vector3.up * 2;
        Vector2 healthBarSize = new Vector3(2, 0.3f);
        healthSystem = HealthBar.Create(10, transform, healthBarOffset, healthBarSize, Color.red, Color.gray, new HealthBar.Border { color = Color.black, thickness = 0.1f });

        inputActions = new GameInputActions();
        inputActions.Game.Enable();
        inputActions.Game.Action1.performed += Action1_performed;

        healthSystem.OnHealthDepleted += HealthSystem_OnHealthDepleted;
        OnReachedDestination += Civilian_OnReachedDestination;
        OnStopMoveing += Civilian_OnReachedDestination;
    }

    private void Action1_performed(InputAction.CallbackContext obj)
    {
        if (!IsSelected) return;

        MoveInputPressed();
    }

    private void Update()
    {
        TestState();
    }

    #endregion


    #region Input

    private void MoveInputPressed()
    {
        if (!GridManager.Instance.grid.IsPositionOnGrid(Mouse2D.GetMousePosition2D())) return;
     
        GridObject gridObject = GridManager.Instance.grid.GetGridObject(Mouse2D.GetMousePosition2D());
        if (gridObject.tilemapSprite != GridObject.TilemapSprite.Minerials)
        {
            state = States.Moving;
            Trigger_OnMove(Mouse2D.GetMousePosition2D());
            return;
        }

        minerial = GridManager.Instance.grid.GetWorldPosition(gridObject.X, gridObject.Y);

        GridObject closestNeighbour = GetClosedtNeighbourFromMinerial(gridObject);
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
            isReturningFromMiningSate = true;
            Trigger_OnMove(townHall.GetLoadTransform().position);
        }
    }

    #endregion


    #region Events

    private void HealthSystem_OnHealthDepleted(object sender, EventArgs e)
    {
        townHall.GetTeamManager().UnitKilled(this);
        Destroy(gameObject);
    }

    private void Civilian_OnReachedDestination(object sender, EventArgs e)
    {
        if (state == States.ReturnFromMining)
        {
            townHall.AddMinerials();

            GridObject closestNeighbour = GetClosedtNeighbourFromMinerial(GridManager.Instance.grid.GetGridObject(minerial));
            Trigger_OnMove(GridManager.Instance.grid.GetWorldPosition(closestNeighbour.X, closestNeighbour.Y));
            state = States.MovingToMine;
            isReturningFromMiningSate = false;
            return;
        }

        state = States.Idle;
    }

    #endregion


    #region Interfaces

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

    public void OnSlot1ButtonClicked()
    {
    }

    public void OnSlot2ButtonClicked()
    {
    }

    public void OnSlot3ButtonClicked()
    {
    }


    public void Damage(int amount)
    {
        healthSystem.Damage(amount);
    }

    #endregion


    #region Other

    public void SetTownHall(TownHall townHall) 
    {
        this.townHall = townHall;
    }

    
    private GridObject GetClosedtNeighbourFromMinerial(GridObject minerialGridObject)
    {
        GridObject closestNeighbour = null;
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
        return closestNeighbour;
    }

    #endregion


}
