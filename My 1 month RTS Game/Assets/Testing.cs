using TheAshBot.Grid;
using TheAshBot.TwoDimentional;

using UnityEngine;

using UnityEngineInternal;


public class Testing : MonoBehaviour
{


    [SerializeField] private TileMapVisual tilemapVisual;


    private TileMap tileMap;
    private TileMap.TileMapObject.TilemapSprite tilemapSprite;


    private void Start()
    {
        tileMap = new TileMap(20, 10, 10, transform.position);

        tileMap.SetTilemapVisual(tilemapVisual);
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mouseWorldPosition = Mouse2D.GetMousePosition2D();
            tileMap.SetTilemapSprite(mouseWorldPosition, tilemapSprite);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tilemapSprite = TileMap.TileMapObject.TilemapSprite.None;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            tilemapSprite = TileMap.TileMapObject.TilemapSprite.Dirt;
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            tilemapSprite = TileMap.TileMapObject.TilemapSprite.Grass;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            tilemapSprite = TileMap.TileMapObject.TilemapSprite.Sky;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            tilemapSprite = TileMap.TileMapObject.TilemapSprite.Stone;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            tilemapSprite = TileMap.TileMapObject.TilemapSprite.Stone;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            tileMap.Save();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            tileMap.Load();
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
