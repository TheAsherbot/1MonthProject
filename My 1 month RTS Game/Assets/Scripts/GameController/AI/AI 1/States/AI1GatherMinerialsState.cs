using System.Collections.Generic;

using UnityEngine;

public class AI1GatherMinerialsState : AI1_BaseState
{


    private enum PotetalActions
    {
        Idle = 0,
        SwitchStateToBuildingArrmy = 1,
        SpawnCivilianAndStartMining = 2,
    }




    private const int NUMBER_OF_ACTIONS = 3;



    public AI1GatherMinerialsState(GameRTSController controller, TeamManager teamManager, TeamManager enemyTeamManager) : base(controller, teamManager, enemyTeamManager)
    {
        this.controller = controller;
        this.teamManager = teamManager;
        this.enemyTeamManager = enemyTeamManager;
    }


    public override AI1_BaseState HandleState()
    {
        // Getting valid actions
        List<int> validChoices = new List<int> { 0, 1 };

        if (teamManager.GetMinerials() >= GameAssets.Instance.AICivilianUnitSO.cost)
        {
            validChoices.Add((int)PotetalActions.SpawnCivilianAndStartMining);
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
            case PotetalActions.SwitchStateToBuildingArrmy:
                // TODO: Add this.
                break;
            case PotetalActions.SpawnCivilianAndStartMining:
                SpawnCivilianAndStartMining();
                break;
        }

        return this;
    }

    private void SpawnCivilianAndStartMining()
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

        // Spowning The Unit
        if (townHall.Spawn(GameAssets.Instance.AICivilianUnitSO, out _BaseUnit unit))
        {
            Civilian civilian = unit as Civilian;
            GridObject minerial = GetClosestMinerialToTownHall(townHall);

            civilian.Move(GridManager.Instance.grid.GetWorldPosition(minerial.X, minerial.Y));
        }
    }

    private GridObject GetClosestMinerialToTownHall(TownHall townHall, int radius = 10)
    {
        Grid grid = GridManager.Instance.grid;
        Vector2 startPosition = grid.SnapPositionToGrid(townHall.GetLoadTransform().position);
        float cellSize = grid.GetCellSize();

        List<GridObject> minerialGridObjectList = new List<GridObject>();

        for (int x = 0; x < radius; x++)
        {
            for (int y = 0; y < radius - x; y++)
            {
                GridObject gridObject;
                // Top Right
                gridObject = grid.GetGridObject(new Vector2(startPosition.x + (cellSize * x), startPosition.y + (cellSize * y)));
                TestIfGridObjectsHasMinerialsAndAdd(gridObject);

                // Top Left
                if (x != 0)
                {
                    gridObject = grid.GetGridObject(new Vector2(startPosition.x - (cellSize * x), startPosition.y + (cellSize * y)));
                    TestIfGridObjectsHasMinerialsAndAdd(gridObject);
                }

                // Bottom
                if (y != 0)
                {
                    // Right
                    gridObject = grid.GetGridObject(new Vector2(startPosition.x + (cellSize * x), startPosition.y - (cellSize * y)));
                    TestIfGridObjectsHasMinerialsAndAdd(gridObject);

                    //Left
                    if (x != 0)
                    {
                        gridObject = grid.GetGridObject(new Vector2(startPosition.x - (cellSize * x), startPosition.y - (cellSize * y)));
                        TestIfGridObjectsHasMinerialsAndAdd(gridObject);
                    }
                }
            }
        }

        GridObject closestMinerialGridObject = null;
        foreach (GridObject minerialGridObject in minerialGridObjectList)
        {
            if (closestMinerialGridObject == null)
            {
                closestMinerialGridObject = minerialGridObject;
                continue;
            }

            if (Mathf.Abs(Vector2.Distance(townHall.GetLoadTransform().position, grid.GetWorldPosition(minerialGridObject.X, minerialGridObject.Y))) < 
                Mathf.Abs(Vector2.Distance(townHall.GetLoadTransform().position, grid.GetWorldPosition(closestMinerialGridObject.X, closestMinerialGridObject.Y))))
            {
                // This is closer than the last one.
                closestMinerialGridObject = minerialGridObject;
            }
        }

        return closestMinerialGridObject;

        void TestIfGridObjectsHasMinerialsAndAdd(GridObject gridObject)
        {
            if (gridObject.tilemapSprite == GridObject.TilemapSprite.Minerials)
            {
                minerialGridObjectList.Add(gridObject);
            }
        }
    }



}
