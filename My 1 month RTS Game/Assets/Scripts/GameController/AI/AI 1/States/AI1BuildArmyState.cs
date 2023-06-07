using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

using TheAshBot;
using TheAshBot.TwoDimentional;

using UnityEngine;

public class AI1BuildArmyState : AI1_BaseState
{


    private enum PotetalActions
    {
        Idle = 0,
        SpawnSwordsMan = 1,
        SpawnSpawnRanger = 2,
        SwitchStateToGatherMinerials = 3,
        SwitchStateToDefending = 4,
        SwitchStateToAttaching = 5,
    }



    private const int NUMBER_OF_ACTIONS = 6;
    private const int MINIMUM_NUMBER_OF_UNITS_TO_SWITCH_TO_ATTACKING_STATE = 10;




    public AI1BuildArmyState(GameRTSController controller, TeamManager teamManager, TeamManager enemyTeamManager) : base(controller, teamManager, enemyTeamManager)
    {
        this.controller = controller;
        this.teamManager = teamManager;
        this.enemyTeamManager = enemyTeamManager;
    }




    public override AI1_BaseState HandleState()
    {
        // Getting valid actions
        List<int> validChoices = new List<int> { 0, 3 };


        if (CanSpawnUnits())
        {
            validChoices.Add(1);
        }

        #region Defending

        List<TownHall> townHallList = new List<TownHall>();
        foreach (_BaseBuilding building in teamManager.GetListOfAllBuildings())
        {
            if (building is TownHall townHall)
            {
                townHallList.Add(townHall);
            }
        }

        float agroRange = 25 * GridManager.Instance.grid.GetCellSize();

        List<AttackingUnit> enemyAttackingUnitsInRange = new List<AttackingUnit>();
        foreach (AttackingUnit AttackingUnit in enemyTeamManager.GetListOfAllAttackingUnits())
        {
            foreach (TownHall townHall in townHallList)
            {
                if (Vector2.Distance(AttackingUnit.transform.position, townHall.transform.position) < agroRange)
                {
                    enemyAttackingUnitsInRange.Add(AttackingUnit);
                    validChoices = new List<int> { 2 };
                }
            }
        }

        #endregion

        if (CanSwitchStateToDefendingState())
        {
            validChoices.Add(4);
        }

        if (CanSwitchToAttackingState())
        {
            validChoices.Add(5);
        }


        // Chosing an action
        int actionNumber = 0;
        bool choseAction = true;
        while (choseAction)
        {
            // Getting random Action
            actionNumber = Random.Range(0, NUMBER_OF_ACTIONS);

            if (validChoices.Contains(actionNumber))
            {
                // Can chose this action
                choseAction = false;
            }
        }
        PotetalActions action = (PotetalActions)actionNumber;

        // Acting on chosen action
        switch (action)
        {
            case PotetalActions.Idle:
                return this;
            case PotetalActions.SpawnSwordsMan:
                SpawnUnit(GameAssets.Instance.AISwordsManUnitSO);
                return this;
            case PotetalActions.SpawnSpawnRanger:
                SpawnUnit(GameAssets.Instance.AIArcherUnitSO);
                return this;
            case PotetalActions.SwitchStateToGatherMinerials:
                return new AI1GatherMinerialsState(controller, teamManager, enemyTeamManager);
            case PotetalActions.SwitchStateToDefending:
                return new AI1DefendingState(controller, teamManager, enemyTeamManager);
            case PotetalActions.SwitchStateToAttaching:
                return new AI1AttackingState(controller, teamManager, enemyTeamManager);
            default:
                return this;
        }
    }


    private bool CanSpawnUnits()
    {
        int minerialsNeededBeforeCanTryToSpawnUnit = 300;

        if (teamManager.GetMinerials() > minerialsNeededBeforeCanTryToSpawnUnit) 
            return true;
        else 
            return false;
    }

    private bool CanSwitchStateToDefendingState()
    {
        if (teamManager.GetListOfAllAttackingUnits().Count < MINIMUM_NUMBER_OF_UNITS_TO_SWITCH_TO_ATTACKING_STATE) 
            return false;


        // Getting all town halls for later
        List<TownHall> townHallList = new List<TownHall>();
        foreach (_BaseBuilding building in teamManager.GetListOfAllBuildings())
        {
            if (building is TownHall townHall)
            {
                townHallList.Add(townHall);
            }
        }

        // Getting all enemys in agro range.
        float distanceBetweenTownHallAndUnitsToAgro = 20f;

        foreach (AttackingUnit AttackingUnit in TeamManager.PlayerInstance.GetListOfAllAttackingUnits())
        {
            foreach (TownHall townHall in townHallList)
            {
                if (Vector2.Distance(AttackingUnit.transform.position, townHall.transform.position) < distanceBetweenTownHallAndUnitsToAgro)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanSwitchToAttackingState()
    {
        if (teamManager.GetListOfAllAttackingUnits().Count < MINIMUM_NUMBER_OF_UNITS_TO_SWITCH_TO_ATTACKING_STATE)
            return false;

        return true;
    }

    private void SpawnUnit(UnitSO unitSO)
    {
        // Choising Random Town Hall
        List<TownHall> townHallList = new List<TownHall>();
        foreach (_BaseBuilding building in teamManager.GetListOfAllBuildings())
        {
            if (building is TownHall)
            {
                townHallList.Add((TownHall)building);
            }
        }
        int townHallIndex = Random.Range(0, townHallList.Count);
        TownHall townHall = townHallList[townHallIndex];

        // Spawning The Unit
        townHall.Spawn(unitSO, OnSpawingUnitFinished);
    }

    private void OnSpawingUnitFinished(_BaseUnit unit, TownHall townHall)
    {
        Vector2 wayPointOffset = new Vector2(-5, -5);
        Vector2 movePosition = (Vector2)townHall.GetLoadTransform().position + wayPointOffset;

        Vector2 boxCastSize = new Vector2(2, 2);
        Vector2 bottomLeft = movePosition - (boxCastSize / 2);
        Vector2 topRight = movePosition + (boxCastSize / 2);

        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(bottomLeft, topRight, GameAssets.Instance.AIUnitLayMask);

        List<ISelectable> selectableList = new List<ISelectable>
        {
            unit as ISelectable,
        };

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.TryGetComponent(out ISelectable selectable)) selectableList.Add(selectable);
        }

        controller.Select(selectableList);

        controller.StartCoroutine(MoveUnitsNextFrame(movePosition));
    }

    private IEnumerator MoveUnitsNextFrame(Vector2 position)
    {
        yield return null;

        controller.MoveSelected(position);
    }

}
