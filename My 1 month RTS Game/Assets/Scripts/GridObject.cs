using System;
using System.Collections.Generic;

using TheAshBot.Grid;
using UnityEngine;

using static TileMap;

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


    public TilemapSprite tilemapSprite
    {
        get;
        private set;
    }


    private Grid grid;
    private int x;
    private int y;


    public GridObject(Grid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }


    public void SetTilemapSprite(TilemapSprite tilemapSprite)
    {
        this.tilemapSprite = tilemapSprite;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return tilemapSprite.ToString();
    }




    /*
     * Save - Load
     */

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


}
