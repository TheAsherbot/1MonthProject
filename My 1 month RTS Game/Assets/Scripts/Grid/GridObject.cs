using System;
using System.Collections.Generic;

using UnityEditor;

public class GridObject
{


    [Serializable]
    public enum TilemapSprite
    {
        None,
        Dirt,
        Building,
        Minerials,
    }

    [Serializable]
    public enum OccupationState
    {
        Empty,
        NotWalkable,
        NotPlaceable,
    }


    #region Veriables


    public TilemapSprite tilemapSprite;


    private Grid grid;
    public int X
    {
        get;
        private set;
    }
    public int Y
    {
        get;
        private set;
    }


    public float gCost;
    public float hCost;
    public float fCost;


    public List<OccupationState> StateList
    {
        get;
        private set;
    }
    public GridObject cameFromNode;

    public List<GridObject> neighbourNodeList;


    #endregion


    public GridObject(Grid grid, int x, int y)
    {
        this.grid = grid;
        X = x;
        Y = y;
        StateList = new List<OccupationState>();
    }


    #region Pathfinding

    public void SetOccupationState(List<OccupationState> stateList)
    {
        StateList = stateList;
        grid.TriggerGridObjectChanged(X, Y);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    #endregion


    public void SetTilemapSprite(TilemapSprite tilemapSprite)
    {
        this.tilemapSprite = tilemapSprite;
        if (tilemapSprite == TilemapSprite.None)
        {
            StateList = new List<OccupationState> { OccupationState.Empty };
        }
        grid.TriggerGridObjectChanged(X, Y);
    }


    public override string ToString()
    {
        string returnedString = "F";
        if (StateList.Contains(OccupationState.NotPlaceable))
        {
            returnedString = "T";
        }
        return returnedString;
    }


    #region Saving, and Loading

    [Serializable]
    public class SaveObject
    {
        public List<OccupationState> StateList;

        public TilemapSprite tilemapSprite;
        public int x;
        public int y;
    }

    public SaveObject Save()
    {
        return new SaveObject
        {
            StateList = StateList,

            tilemapSprite = tilemapSprite,
            x = X,
            y = Y,
        };
    }

    public void Load(SaveObject saveObject)
    {
        if (saveObject.tilemapSprite == default)
        {
            saveObject.tilemapSprite = TilemapSprite.None;
        }
        else if (saveObject.tilemapSprite == TilemapSprite.Minerials)
        {
            SetOccupationState(new List<OccupationState> { OccupationState.NotWalkable, OccupationState.NotPlaceable });
        }
        SetTilemapSprite(saveObject.tilemapSprite);
    }

    #endregion

}
