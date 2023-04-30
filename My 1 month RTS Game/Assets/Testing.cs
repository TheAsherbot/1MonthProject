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
        float cellSize = 10f;
        pathfinding = new Pathfinding(width, height, cellSize, Vector2.zero, true, null);

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
            Debug.Log(Time.deltaTime);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i], path[i + 1], Color.green, 5f);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = Mouse2D.GetMousePosition2D();
            pathfinding.GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetPathNode(x, y).SetIsWalkable(!pathfinding.GetPathNode(x, y).isWalkable);
        }
    }
    

}
