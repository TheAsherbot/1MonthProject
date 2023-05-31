using System.Collections;
using System.Collections.Generic;
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
    private const int MINIMUM_NUMBER_OF_UNITS_TO_SWITCH_TO_DEFENDING_OR_ATTACKING_STATES = 10;




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
            validChoices.Add(2);
        }

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
                SpawnSwordsMan();
                return this;
            case PotetalActions.SpawnSpawnRanger:
                SpawnArcher();
                return this;
            case PotetalActions.SwitchStateToGatherMinerials:
                return new AI1GatherMinerialsState(controller, teamManager, enemyTeamManager);
            case PotetalActions.SwitchStateToDefending:

                break;
            case PotetalActions.SwitchStateToAttaching:
                return new AI1AttackingState(controller, teamManager, enemyTeamManager);
            default:
                return this;
        }

        return this;
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
        if (teamManager.GetListOfAllAttackingUnitUnits().Count < MINIMUM_NUMBER_OF_UNITS_TO_SWITCH_TO_DEFENDING_OR_ATTACKING_STATES) 
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

        foreach (AttackingUnit AttackingUnit in TeamManager.PlayerInstance.GetListOfAllAttackingUnitUnits())
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
        if (teamManager.GetListOfAllAttackingUnitUnits().Count < MINIMUM_NUMBER_OF_UNITS_TO_SWITCH_TO_DEFENDING_OR_ATTACKING_STATES)
            return false;

        return true;
    }

    private void SpawnSwordsMan()
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
        if (townHall.Spawn(GameAssets.Instance.AISwordsManUnitSO, out _BaseUnit unit))
        {
            controller.Select(new List<ISelectable> { (ISelectable)unit });

            Vector2 wayPointOffset = new Vector2(-5, -5);

            controller.MoveSelected((Vector2)townHall.GetLoadTransform().position - wayPointOffset);
        }
    }

    private void SpawnArcher()
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
        if (townHall.Spawn(GameAssets.Instance.AIArcherUnitSO, out _BaseUnit unit))
        {
            controller.Select(new List<ISelectable> { (ISelectable)unit });

            Vector2 wayPointOffset = new Vector2(-5, -5);

            controller.MoveSelected((Vector2)townHall.GetLoadTransform().position - wayPointOffset);
        }
    }


}
