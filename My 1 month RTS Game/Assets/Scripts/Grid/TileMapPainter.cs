using System.Collections;
using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class TileMapPainter : MonoBehaviour
{

    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellSize;
    
    [Space(5)]

    [SerializeField] bool loadOnStart;
    [SerializeField] TextAsset saveData;

    [Space(5)]

    [SerializeField] GridVisual gridVisual;


    private GridObject.TilemapSprite tilemapSprite;
    private Grid grid;


    private void Start()
    {
        if (loadOnStart)
        {
            grid = Grid.Load(saveData.text);
            gridVisual.SetGrid(grid);
            return;
        }

        grid = new Grid(width, height, cellSize, Vector3.zero, false);
        gridVisual.SetGrid(grid);
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Draw();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tilemapSprite = GridObject.TilemapSprite.None;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tilemapSprite = GridObject.TilemapSprite.Dirt;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tilemapSprite = GridObject.TilemapSprite.Building;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            tilemapSprite = GridObject.TilemapSprite.Minerials;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            grid.Save();
        }
    }


    private void Draw()
    {
        Vector2 mousePosition = Mouse2D.GetMousePosition2D();
        grid.SetTilemapSprite(mousePosition, tilemapSprite);
        if (tilemapSprite == GridObject.TilemapSprite.None)
        {
            grid.GetGridObject(mousePosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.Empty } );
        }
        else if (tilemapSprite == GridObject.TilemapSprite.Dirt)
        {
            grid.GetGridObject(mousePosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.Empty });
        }
        else if (tilemapSprite == GridObject.TilemapSprite.Building)
        {
            grid.GetGridObject(mousePosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.NotWalkable, GridObject.OccupationState.NotPlaceable });
        }
        else if (tilemapSprite == GridObject.TilemapSprite.Minerials)
        {

        }
    }


}
