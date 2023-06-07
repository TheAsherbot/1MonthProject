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
        Debug.Log("AI1DefendingState");
        this.controller = controller;
        this.teamManager = teamManager;
        this.enemyTeamManager = enemyTeamManager;
    }



    public override AI1_BaseState HandleState()
    {
        Debug.Log("AI1DefendingState: HandleState");
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

        Debug.Log("AI1DefendingState: action == " + action);

        switch (action)
        {
            case PotetalActions.Idle:
                Debug.Log("AI1DefendingState: Idle");
                return this;
            case PotetalActions.SwitchStateToGatherMinerials:
                Debug.Log("AI1DefendingState: SwitchStateToGatherMinerials");
                return new AI1GatherMinerialsState(controller, teamManager, enemyTeamManager);
            case PotetalActions.Attack:
                Debug.Log("AI1DefendingState: Attack");
                Attack(enemyAttackingUnitsInRange);
                return this;
        }

        return this;
    }

    private void Attack(List<AttackingUnit> enemyAttackingUnitsInRange)
    {
        Debug.Log("AI1DefendingState: Attack Function");
        List<ISelectable> attackingUnitsInRangeAsISelectable = new List<ISelectable>();

        foreach (AttackingUnit unit in teamManager.GetListOfAllAttackingUnits())
        {
            attackingUnitsInRangeAsISelectable.Add(unit);
        }

        controller.Select(attackingUnitsInRangeAsISelectable);

        Debug.Log("AI1DefendingState: Attacking Units");

        controller.MoveSelected(enemyAttackingUnitsInRange[0].transform.position);
    }


}
