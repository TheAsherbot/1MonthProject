using System.Collections;
using System.Collections.Generic;

using TheAshBot.Grid;
using TheAshBot.TwoDimentional;

using Unity.VisualScripting;

using UnityEditor;

using UnityEngine;

public class Testing : MonoBehaviour
{
    
    
    [SerializeField] private Transform hexPrefab;

    private int x;
    private int y;
    private Pathfinding pathfinding;


    private void Start()
    {
        int width = 15;
        int height = 15;
        // float cellSize = 1f;
        pathfinding = new Pathfinding(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Instantiate(hexPrefab, grid.GetWorldPosition(x, y), Quaternion.identity);
            }
        }

    }

    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Mouse2D.GetMousePosition2D();
            List<Vector2> path = pathfinding.FindPathAsVector2s(Vector2.zero, mouseWorldPosition);
            //List<PathNode> path = pathfinding.FindPathAsPathNodes(0, 0, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i], path[i + 1], Color.green, 5f);
                    //Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = Mouse2D.GetMousePosition2D();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }
    

}
