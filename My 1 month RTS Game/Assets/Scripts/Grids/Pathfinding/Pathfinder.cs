using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    
    public Pathfinding pathfindingGrid
    {
        get;
        private set;
    }

    [SerializeField] private int width = 10;
    public int Width
    {
        get
        {
            return width;
        }
    }
    [SerializeField] private int height = 10;
    public int Height
    {
        get
        {
            return height;
        }
    }
    [SerializeField] private float cellSize = 1;
    public float CellSize
    {
        get
        {
            return cellSize;
        }
    }

    
    [SerializeField] private bool debug;
    private bool last_debug;


    private GameObject parent;


    private void OnValidate()
    {
        if (last_debug != debug && !debug)
        {
            Debug.Log("last_debug");
            // Needs to destroy the gameobject
            if (parent != null)
            {
                Debug.Log("parent does not equal null");
                DestroyImmediate(parent);
            }
            last_debug = debug;
        }
    }


    public void BakePathfindingGrid()
    {
        if (debug)
        {
            if (parent == null)
            {
                parent = new GameObject("Pathfinding parent");
            }

            if (pathfindingGrid != null)
            {
                pathfindingGrid.DestorySelf();
            }

            pathfindingGrid = new Pathfinding(width, height, cellSize, transform.position, true, parent.transform);
        }

    }

    
}
