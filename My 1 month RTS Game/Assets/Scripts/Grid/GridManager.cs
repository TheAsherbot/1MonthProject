using System;
using System.IO;

using TheAshBot.Grid;
using TheAshBot.TwoDimentional;

using UnityEngine;

using UnityEngineInternal;


public class GridManager : MonoBehaviour
{


    public static GridManager Instance;


    public Grid Grid
    {
        get;
        private set;
    }


    [Header("Grid")]
    [SerializeField] private int xSize = 20;
    [SerializeField] private int ySize = 10;
    [SerializeField] private int cellSize = 10;

    [SerializeField] private GridVisual gridVisual;
    [SerializeField] private TextAsset gridSavedData;


    private GridObject.TilemapSprite gridSprite = GridObject.TilemapSprite.Dirt;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GridManager Instance in this scene");
        }
        else
        {
            Instance = this;
        }

        Grid = new Grid(xSize, ySize, cellSize, transform.position, false, true, null);
    }

    private void Start()
    {
        gridVisual.SetGrid(Grid);
    }

    private void Update()
    {

    }



    private void TileMap()
    {
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
        {
            Vector2 mouseWorldPosition = Mouse2D.GetMousePosition2D();
            Grid.SetTilemapSprite(mouseWorldPosition, gridSprite);
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
    }

    private void SaveLoad()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Grid.Save();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            Grid.Load(gridSavedData.text);
        }
    }


}
