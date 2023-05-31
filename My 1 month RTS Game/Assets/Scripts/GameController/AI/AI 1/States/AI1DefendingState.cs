using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI1DefendingState : AI1_BaseState
{


    private enum PotetalActions
    {
        Idle = 0,
        SwitchStateToGatherMinerials = 1,
        Attack = 2,
    }




    private const int NUMBER_OF_ACTIONS = 3;



    public AI1DefendingState(GameRTSController controller, TeamManager teamManager, TeamManager enemyTeamManager) : base(controller, teamManager, enemyTeamManager)
    {
        this.controller = controller;
        this.teamManager = teamManager;
        this.enemyTeamManager = enemyTeamManager;
    }



    public override AI1_BaseState HandleState()
    {
        // Getting valid actions
        List<int> validChoices = new List<int> { 0, 1 };

        // Getting all town halls for later
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
        foreach (AttackingUnit AttackingUnit in TeamManager.PlayerInstance.GetListOfAllAttackingUnitUnits())
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


        switch (action)
        {
            case PotetalActions.Idle:
                return this;
            case PotetalActions.SwitchStateToGatherMinerials:
                return new AI1GatherMinerialsState(controller, teamManager, enemyTeamManager);
            case PotetalActions.Attack:
                Attack(enemyAttackingUnitsInRange);
                return this;
        }

        return this;
    }

    private void Attack(List<AttackingUnit> enemyAttackingUnitsInRange)
    {
        List<ISelectable> attackingUnitsInRangeAsISelectable = new List<ISelectable>();

        foreach (AttackingUnit unit in teamManager.GetListOfAllAttackingUnitUnits())
        {
            attackingUnitsInRangeAsISelectable.Add(unit);
        }

        controller.Select(attackingUnitsInRangeAsISelectable);

        controller.MoveSelected(enemyAttackingUnitsInRange[0].transform.position);
    }


}
