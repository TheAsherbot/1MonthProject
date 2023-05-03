using System;
using System.Collections.Generic;

public class GridObject
{


    public enum TilemapSprite
    {
        None,
        Dirt,
        Grass,
        Sky,
        Stone,
    }


    #region Veriables

    public TilemapSprite tilemapSprite;


    private Grid grid;
    public int x;
    public int y;


    public float gCost;
    public float hCost;
    public float fCost;

    public bool isWalkable;
    public GridObject cameFromNode;

    public List<GridObject> neighbourNodeList;

    #endregion


    public GridObject(Grid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }


    #region Pathfinding

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    #endregion


    public void SetTilemapSprite(TilemapSprite tilemapSprite)
    {
        this.tilemapSprite = tilemapSprite;
        grid.TriggerGridObjectChanged(x, y);
    }


    public override string ToString()
    {
        return tilemapSprite.ToString();
    }


    #region Saving, and Loading

    [Serializable]
    public class SaveObject
    {
        public TilemapSprite tilemapSprite;
        public int x;
        public int y;
    }

    public SaveObject Save()
    {
        return new SaveObject
        {
            tilemapSprite = tilemapSprite,
            x = x,
            y = y,
        };
    }

    public void Load(SaveObject saveObject)
    {
        if (saveObject.tilemapSprite == default)
        {
            tilemapSprite = TilemapSprite.None;
        }
        tilemapSprite = saveObject.tilemapSprite;
    }

    #endregion

}
