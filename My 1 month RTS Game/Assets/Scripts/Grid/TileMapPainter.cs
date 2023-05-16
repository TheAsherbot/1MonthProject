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


    private bool isDrawing;
    private GridObject.TilemapSprite tilemapSprite = GridObject.TilemapSprite.None;
    private Grid grid;

    private LevelEditorInputActions inputActions;


    private void Start()
    {
        inputActions = new LevelEditorInputActions();
        inputActions.Enable();
        inputActions.LevelEditor.Paint.started += Paint_started;
        inputActions.LevelEditor.Paint.canceled += Paint_canceled;
        inputActions.LevelEditor.TileMapSprite1.started += TileMapSprite1_started;
        inputActions.LevelEditor.TileMapSprite2.started += TileMapSprite2_started;
        inputActions.LevelEditor.TileMapSprite3.started += TileMapSprite3_started;
        inputActions.LevelEditor.TileMapSprite4.started += TileMapSprite4_started;
        inputActions.LevelEditor.TileMapSprite5.started += TileMapSprite5_started;
        inputActions.LevelEditor.Save.started += Save_started;

        if (loadOnStart)
        {
            grid = Grid.Load(saveData.text);
            gridVisual.SetGrid(grid);
            return;
        }

        grid = new Grid(width, height, cellSize, Vector3.zero, false);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid.SetTilemapSprite(x, y, GridObject.TilemapSprite.Dirt);
            }
        }
        gridVisual.SetGrid(grid);
    }

    private void Save_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        grid.Save();
    }

    private void TileMapSprite1_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        tilemapSprite = GridObject.TilemapSprite.None;
    }

    private void TileMapSprite2_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        tilemapSprite = GridObject.TilemapSprite.Dirt;
    }

    private void TileMapSprite3_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        tilemapSprite = GridObject.TilemapSprite.Rocks;
    }

    private void TileMapSprite4_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        tilemapSprite = GridObject.TilemapSprite.Minerials;
    }

    private void TileMapSprite5_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        tilemapSprite = GridObject.TilemapSprite.Building;
    }

    private void Paint_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isDrawing = true;
    }

    private void Paint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isDrawing = false;
    }


    private void Update()
    {
        if (isDrawing)
        {
            Draw();
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
        else if (tilemapSprite == GridObject.TilemapSprite.Rocks)
        {
            grid.GetGridObject(mousePosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.NotWalkable, GridObject.OccupationState.NotPlaceable });
        }
        else if (tilemapSprite == GridObject.TilemapSprite.Minerials)
        {
            grid.GetGridObject(mousePosition).SetOccupationState(new List<GridObject.OccupationState> { GridObject.OccupationState.NotWalkable, GridObject.OccupationState.NotPlaceable });
        }
    }


}
