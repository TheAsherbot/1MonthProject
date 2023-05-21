using System;
using System.Collections;
using System.Collections.Generic;

using TheAshBot;
using TheAshBot.Grid;

using UnityEngine;

public class TileMap
{


    public event EventHandler OnLoaded;


    public GenericGrid<TileMapObject> Grid
    {
        get;
        private set;
    }


    public TileMap(int width, int height, int cellSize, Vector2 originPosition)
    {
        Grid = new GenericGrid<TileMapObject>(width, height, cellSize, originPosition, 
            (GenericGrid<TileMapObject> grid, int x, int y) => new TileMapObject(grid, x ,y), false, null);
    }

    public void SetTilemapSprite(Vector2 worldPosition, TileMapObject.TilemapSprite tilemapSprite)
    {
        TileMapObject tilemapObject = Grid.GetGridObject(worldPosition);
        if (tilemapObject != null)
        {
            tilemapObject.SetTilemapSprite(tilemapSprite);
        }
    }
    public void SetTilemapSprite(int x, int y, TileMapObject.TilemapSprite tilemapSprite)
    {
        TileMapObject tilemapObject = Grid.GetGridObject(x, y);
        if (tilemapObject != null)
        {
            tilemapObject.SetTilemapSprite(tilemapSprite);
        }
    }

    public void SetTilemapVisual(TileMapVisual tilemapVisual)
    {
        tilemapVisual.SetGrid(this, Grid);
    }



    /*
     * Save - Load
     */

    public void Save()
    {
        List<TileMapObject.SaveObject> tilemapObjectSaveObjectList = new List<TileMapObject.SaveObject>();
        for (int x = 0; x < Grid.GetWidth(); x++)
        {
            for (int y = 0; y < Grid.GetHeight(); y++)
            {
                TileMapObject tilemapObject = Grid.GetGridObject(x, y);
                if (tilemapObject.tilemapSprite != TileMapObject.TilemapSprite.None)
                {
                    tilemapObjectSaveObjectList.Add(tilemapObject.Save());
                }
            }
        }

        SaveObject saveObject = new SaveObject
        {
            tilemapObjectSaveObjectArray = tilemapObjectSaveObjectList.ToArray(),
        };

        SaveSystem.SaveJson(saveObject, SaveSystem.RootSavePath.DataPath, "Tile Map", "tilemap", true);
    }

    public void Load(string saveObjectJsonData)
    {
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveObjectJsonData);
        foreach (TileMapObject.SaveObject tilemapObjectSaveObject in saveObject.tilemapObjectSaveObjectArray)
        {
            TileMapObject tilemapObject = Grid.GetGridObject(tilemapObjectSaveObject.x, tilemapObjectSaveObject.y);
            tilemapObject.Load(tilemapObjectSaveObject);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }

    public class SaveObject
    {
        public TileMapObject.SaveObject[] tilemapObjectSaveObjectArray;
    }






    /*
     * Represents a single Tilemap Object that exists in each Grid Cell Position
     * */

    public class TileMapObject
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


        private GenericGrid<TileMapObject> grid;
        private int x;
        private int y;


        public TileMapObject(GenericGrid<TileMapObject> grid, int x, int y)
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


}
