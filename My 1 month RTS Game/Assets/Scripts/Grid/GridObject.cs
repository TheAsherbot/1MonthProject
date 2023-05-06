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

    [Serializable, Flags]
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


    public OccupationState State
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
        State = OccupationState.Empty;
    }


    #region Pathfinding

    public void SetOccupationState(OccupationState state)
    {
        State = state;
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
        if (tilemapSprite != TilemapSprite.None)
        {
            State = OccupationState.NotWalkable & OccupationState.NotPlaceable;

        }
        else
        {
            State = OccupationState.Empty;
        }
        grid.TriggerGridObjectChanged(X, Y);
    }


    public override string ToString()
    {
        return State == OccupationState.NotWalkable ? "T" : "F";
    }


    #region Saving, and Loading

    [Serializable]
    public class SaveObject
    {
        public OccupationState State;

        public TilemapSprite tilemapSprite;
        public int x;
        public int y;
    }

    public SaveObject Save()
    {
        return new SaveObject
        {
            State = State,

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
        SetTilemapSprite(saveObject.tilemapSprite);
    }

    #endregion

}
