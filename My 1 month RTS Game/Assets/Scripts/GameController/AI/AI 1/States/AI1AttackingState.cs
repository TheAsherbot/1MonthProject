using System.Collections.Generic;

using UnityEngine;

public class AI1AttackingState : AI1_BaseState
{


    private enum PotetalActions
    {
        Idle = 0,
        SwitchStateToGatherMinerials = 1,
        Attack = 2,
    }



    private const int NUMBER_OF_ACTIONS = 3;



    public AI1AttackingState(GameRTSController controller, TeamManager teamManager, TeamManager enemyTeamManager) : base(controller, teamManager, enemyTeamManager)
    {
        this.controller = controller;
        this.teamManager = teamManager;
        this.enemyTeamManager = enemyTeamManager;
    }


    public override AI1_BaseState HandleState()
    {

        int actionNumber = Random.Range(0, NUMBER_OF_ACTIONS);

        PotetalActions action = (PotetalActions)actionNumber;

        switch (action)
        {
            case PotetalActions.Idle:
                return this;
            case PotetalActions.SwitchStateToGatherMinerials:
                return new AI1GatherMinerialsState(controller, teamManager, enemyTeamManager);
            case PotetalActions.Attack:
                List<TownHall> townHallList = new List<TownHall>();
                foreach (_BaseBuilding building in enemyTeamManager.GetListOfAllBuildings())
                {
                    if (building is TownHall)
                    {
                        townHallList.Add((TownHall)building);
                    }
                }
                TownHall curentTownHall = townHallList[Random.Range(0, townHallList.Count)];

                List<ISelectable> attackingUnitsAsIselectableList = new List<ISelectable>();
                foreach (AttackingUnit unit in teamManager.GetListOfAllAttackingUnits())
                {
                    attackingUnitsAsIselectableList.Add(unit);
                }

                controller.Select(attackingUnitsAsIselectableList);
                controller.MoveSelected(curentTownHall.transform.position);

                return this;
        }

        return this;
    }

}
