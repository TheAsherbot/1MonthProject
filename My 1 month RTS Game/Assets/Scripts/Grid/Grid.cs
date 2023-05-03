using System.Collections.Generic;
using System;
using TheAshBot.Grid;
using UnityEngine;

public class Grid
{

    #region Events

    public event EventHandler OnLoaded;
    /// <summary>
    /// This holds all the functions that are called when the grid changes
    /// </summary>
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    /// <summary>
    /// This triggers the grid changing
    /// </summary>
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    #endregion


    #region variables

    private int width;
    private int height;
    private float cellSize;
    private Vector2 originPosition;
    private GridObject[,] gridArray;


    private float moveStraightCost
    {
        get
        {
            return 1f * cellSize;
        }
    }
    private float moveDiagonalCost
    {
        get
        {
            return 1.4f * cellSize;
        }
    }

    private List<GridObject> openList;
    private List<GridObject> closedList;


    #endregion


    public Grid(int width, int height, int cellSize, Vector2 originPosition, bool showDebug, Transform parent)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new GridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridObject(this, x, y);
            }
        }

        // Doing it agian becouse now all fo the path nodes have been made.
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y].neighbourNodeList = GetNeighbourList(gridArray[x, y]);
            }
        }

        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = CreateWorldText(parent, gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * .5f, 5 * (int)cellSize, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
    }
    public Grid(int width, int height, int cellSize, Vector2 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new GridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridObject(this, x, y);
            }
        }

        // Doing it agian becouse now all fo the path nodes have been made.
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y].neighbourNodeList = GetNeighbourList(gridArray[x, y]);
            }
        }
    }


    public void SetTilemapSprite(Vector2 worldPosition, GridObject.TilemapSprite tilemapSprite)
    {
        GridObject gridObject = GetGridObject(worldPosition);
        if (gridObject != null)
        {
            gridObject.SetTilemapSprite(tilemapSprite);
        }
    }
    public void SetTilemapSprite(int x, int y, GridObject.TilemapSprite tilemapSprite)
    {
        GridObject gridObject = GetGridObject(x, y);
        if (gridObject != null)
        {
            gridObject.SetTilemapSprite(tilemapSprite);
        }
    }

    public void SetGridVisual(GridVisual gridVisual)
    {
        gridVisual.SetGrid(this);
    }


    #region Find Path

    public List<Vector2> FindPathAsVector2s(int startX, int startY, int endX, int endY)
    {
        List<GridObject> path = FindPathAsGridObjects(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector2> vectorPath = new List<Vector2>();
            foreach (GridObject gridObject in path)
            {
                vectorPath.Add(new Vector2(gridObject.x, gridObject.y) * GetCellSize() + Vector2.one * GetCellSize() * 0.5f + originPosition);
            }
            return vectorPath;
        }
    }
    public List<Vector2> FindPathAsVector2s(int startX, int startY, Vector2 endWorldPosition)
    {
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsVector2s(startX, startY, endX, endY);
    }
    public List<Vector2> FindPathAsVector2s(Vector2 startWorldPosition, Vector2 endWorldPosition)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsVector2s(startX, startY, endX, endY);
    }
    public List<Vector2> FindPathAsVector2s(Vector2 startWorldPosition, int endX, int endY)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        return FindPathAsVector2s(startX, startY, endX, endY);
    }


    public List<GridObject> FindPathAsGridObjects(int startX, int startY, int endX, int endY)
    {
        GridObject startNode = GetGridObject(startX, startY);
        GridObject endNode = GetGridObject(endX, endY);

        if (endNode == null)
        {
            // End node is not on the grid
            return default;
        }

        openList = new List<GridObject>() { startNode };
        closedList = new List<GridObject>();

        for (int x = 0; x < GetWidth(); x++)
        {
            for (int y = 0; y < GetHeight(); y++)
            {
                GridObject pathNode = GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            GridObject currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached finel node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GridObject neighbourNode in currentNode.neighbourNodeList)
            {
                if (closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                float tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);

                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the open lsit
        return null;
    }
    public List<GridObject> FindPathAsGridObjects(Vector2 startWorldPosition, int endX, int endY)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        return FindPathAsGridObjects(startX, startY, endX, endY);
    }
    public List<GridObject> FindPathAsGridObjects(Vector2 startWorldPosition, Vector2 endWorldPosition)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsGridObjects(startX, startY, endX, endY);
    }
    public List<GridObject> FindPathAsGridObjects(int startX, int startY, Vector2 endWorldPosition)
    {
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsGridObjects(startX, startY, endX, endY);
    }

    #endregion


    #region Pathfinding

    private List<GridObject> GetNeighbourList(GridObject currentNode)
    {
        List<GridObject> neighbourList = new List<GridObject>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetGridObject(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetGridObject(currentNode.x - 1, currentNode.y - 1));
            }
            // Left Up
            if (currentNode.y + 1 < GetHeight())
            {
                neighbourList.Add(GetGridObject(currentNode.x - 1, currentNode.y + 1));
            }
        }
        if (currentNode.x + 1 < GetWidth())
        {
            // Right
            neighbourList.Add(GetGridObject(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetGridObject(currentNode.x + 1, currentNode.y - 1));
            }
            // Right Up
            if (currentNode.y + 1 < GetHeight())
            {
                neighbourList.Add(GetGridObject(currentNode.x + 1, currentNode.y + 1));
            }
        }
        // Bottom
        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(GetGridObject(currentNode.x, currentNode.y - 1));
        }
        // Top
        if (currentNode.y + 1 < GetHeight())
        {
            neighbourList.Add(GetGridObject(currentNode.x, currentNode.y + 1));
        }

        return neighbourList;
    }

    private List<GridObject> CalculatePath(GridObject endNode)
    {
        List<GridObject> path = new List<GridObject>();
        path.Add(endNode);
        GridObject currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private float CalculateDistance(GridObject a, GridObject b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return moveDiagonalCost * Mathf.Min(xDistance, yDistance) + moveStraightCost * remaining;
    }

    private GridObject GetLowestFCostNode(List<GridObject> gridObjectList)
    {
        GridObject lowestFCostNode = gridObjectList[0];

        for (int i = 1; i < gridObjectList.Count; i++)
        {
            if (gridObjectList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = gridObjectList[i];
            }
        }

        return lowestFCostNode;
    }

    #endregion


    #region Basic Grid

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    /// <summary>
    /// This gets the world position of a grid object using its x, and y position
    /// </summary>
    /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
    /// <param name="y">This is the number of grid objects above the start grid object</param>
    /// <returns>The world position</returns>
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize + originPosition;
    }

    /// <summary>
    /// This gets the x and y position of a grid object using it's world position
    /// </summary>
    /// <param name="worldPosition">This is the world position of the grid object</param>
    /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
    /// <param name="y">This is the number of grid objects above the start grid object</param>
    public void GetXY(Vector2 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    /// <summary>
    /// This snaps a position to the grid
    /// </summary>
    /// <param name="worldPosition">This is the world position of the grid object</param>
    /// <returns>Returns the world position snaped to the grid</returns>
    public Vector2 SnapPositionToGrid(Vector2 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        return new Vector2(x, y);
    }

    /// <summary>
    /// This sets the value of a cell using it's 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetGridObject(int x, int y, GridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (value == null)
            {
                value = default;
            }

            gridArray[x, y] = value;

            TriggerGridObjectChanged(x, y);
        }
    }

    /// <summary>
    /// This sets the vaue of a cell using it's world position
    /// </summary>
    /// <param name="worldPosition">This is the grid objects world position<</param>
    /// <param name="value">This is the value it is being set to</param>
    public void SetGridObject(Vector2 worldPosition, GridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    /// <summary>
    /// This gets the value of a cell using it's positon on the grid
    /// </summary>
    /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
    /// <param name="y">This is the number of grid objects above the start grid object</param>
    /// <returns>Returns the grid object</returns>
    public GridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// This gets the value of a cell using it's world position
    /// </summary>
    /// <param name="worldPosition">This is the grid objects world position</param>
    /// <returns>This ruterns the grid object</returns>
    public GridObject GetGridObject(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs
        {
            x = x,
            y = y
        });
    }

    #endregion


    #region Saveing, and Loading

    public void Save()
    {
        List<GridObject.SaveObject> gridObjectSaveObjectList = new List<GridObject.SaveObject>();
        for (int x = 0; x < GetWidth(); x++)
        {
            for (int y = 0; y < GetHeight(); y++)
            {
                GridObject gridObject = GetGridObject(x, y);
                if (gridObject.tilemapSprite != GridObject.TilemapSprite.None)
                {
                    gridObjectSaveObjectList.Add(gridObject.Save());
                }
            }
        }

        SaveObject saveObject = new SaveObject
        {
            gridObjectSaveObjectArray = gridObjectSaveObjectList.ToArray(),
        };

        SaveSystem.SaveJson(saveObject, SaveSystem.RootSavePath.DataPath, "Tile Map", "tilemap", true);
    }

    public void Load(string saveObjectJsonData)
    {
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveObjectJsonData);
        foreach (GridObject.SaveObject gridObjectSaveObject in saveObject.gridObjectSaveObjectArray)
        {
            GridObject gridObject = GetGridObject(gridObjectSaveObject.x, gridObjectSaveObject.y);
            
            gridObject.Load(gridObjectSaveObject);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }

    public class SaveObject
    {
        public GridObject.SaveObject[] gridObjectSaveObjectArray;
    }

    #endregion


    #region Helpers

    public static TextMesh CreateWorldText(Transform parent, string text, Vector2 localPosition, int fontSize, Color color, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }

    #endregion

}
