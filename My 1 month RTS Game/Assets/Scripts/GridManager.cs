using System;
using System.IO;

using TheAshBot.Grid;
using TheAshBot.TwoDimentional;

using UnityEngine;

using UnityEngineInternal;


public class GridManager : MonoBehaviour
{


    [SerializeField] private GridVisual gridVisual;
    [SerializeField] private TextAsset gridSavedData;


    private Grid grid;
    private GridObject.TilemapSprite gridSprite;




    private void Start()
    {
        grid = new Grid(20, 10, 10, transform.position);

        gridVisual.SetGrid(grid);

        //grid.Load(gridSavedData.text);
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mouseWorldPosition = Mouse2D.GetMousePosition2D();
            grid.SetTilemapSprite(mouseWorldPosition, gridSprite);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            gridSprite = GridObject.TilemapSprite.None;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            gridSprite = GridObject.TilemapSprite.Dirt;
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            gridSprite = GridObject.TilemapSprite.Grass;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            gridSprite = GridObject.TilemapSprite.Sky;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            gridSprite = GridObject.TilemapSprite.Stone;
        }

        // Saving
        if (Input.GetKeyDown(KeyCode.B))
        {
            grid.Save();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            grid.Load(gridSavedData.text);
        }
    }




    /*
    private void DrawPathfingLines()
    {
        Vector3 mouseWorldPosition = Mouse2D.GetMousePosition2D();
        List<Vector2> path = pathfinding.FindPathAsVector2s(Vector2.zero, mouseWorldPosition);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.green, 5f);
            }
        }
    }
    */

}
