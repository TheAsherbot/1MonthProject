using System.Collections.Generic;
using System;
using TheAshBot.Grid;
using UnityEngine;
using Unity.VisualScripting;

public class Grid
{

    [Serializable]
    public struct BuildingLayer
    {
        public int width;
        public List<GridObject.OccupationState> occupationState;
    }


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
    private bool canWalkDiagonally;
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


    public Grid(int width, int height, float cellSize, Vector2 originPosition, bool canWalkDiagonally, bool showDebug, Transform parent)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.canWalkDiagonally = canWalkDiagonally;

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
    public Grid(int width, int height, float cellSize, Vector2 originPosition, bool canWalkDiagonally)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.canWalkDiagonally = canWalkDiagonally;

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


    #region Find Path

    public List<Vector2> FindPathAsVector2s(int startX, int startY, int endX, int endY, List<GridObject.OccupationState> unwalkableStates)
    {
        List<GridObject> path = FindPathAsGridObjects(startX, startY, endX, endY, unwalkableStates);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector2> vectorPath = new List<Vector2>();
            foreach (GridObject gridObject in path)
            {
                vectorPath.Add(new Vector2(gridObject.X, gridObject.Y) * GetCellSize() + Vector2.one * GetCellSize() * 0.5f + originPosition);
            }
            return vectorPath;
        }
    }
    public List<Vector2> FindPathAsVector2s(int startX, int startY, Vector2 endWorldPosition, List<GridObject.OccupationState> unwalkableStates)
    {
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsVector2s(startX, startY, endX, endY, unwalkableStates);
    }
    public List<Vector2> FindPathAsVector2s(Vector2 startWorldPosition, Vector2 endWorldPosition, List<GridObject.OccupationState> unwalkableStates)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsVector2s(startX, startY, endX, endY, unwalkableStates);
    }
    public List<Vector2> FindPathAsVector2s(Vector2 startWorldPosition, int endX, int endY, List<GridObject.OccupationState> unwalkableStates)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        return FindPathAsVector2s(startX, startY, endX, endY, unwalkableStates);
    }


    public List<GridObject> FindPathAsGridObjects(int startX, int startY, int endX, int endY, List<GridObject.OccupationState> unwalkableStates)
    {
        GridObject startNode = GetGridObject(startX, startY);
        GridObject endNode = GetGridObject(endX, endY);

        if (endNode == null || endNode.State == GridObject.OccupationState.NotWalkable)
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

                    if (unwalkableStates.Contains(neighbourNode.State))
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
    public List<GridObject> FindPathAsGridObjects(Vector2 startWorldPosition, int endX, int endY, List<GridObject.OccupationState> unwalkableStates)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        return FindPathAsGridObjects(startX, startY, endX, endY, unwalkableStates);
    }
    public List<GridObject> FindPathAsGridObjects(Vector2 startWorldPosition, Vector2 endWorldPosition, List<GridObject.OccupationState> unwalkableStates)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsGridObjects(startX, startY, endX, endY, unwalkableStates);
    }
    public List<GridObject> FindPathAsGridObjects(int startX, int startY, Vector2 endWorldPosition, List<GridObject.OccupationState> unwalkableStates)
    {
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsGridObjects(startX, startY, endX, endY, unwalkableStates);
    }


    #endregion


    #region Pathfinding

    private List<GridObject> GetNeighbourList(GridObject currentNode)
    {
        List<GridObject> neighbourList = new List<GridObject>();

        if (currentNode.X - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetGridObject(currentNode.X - 1, currentNode.Y));
            if (canWalkDiagonally)
            {
                // Left Down
                if (currentNode.Y - 1 >= 0)
                {
                    neighbourList.Add(GetGridObject(currentNode.X - 1, currentNode.Y - 1));
                }
                // Left Up
                if (currentNode.Y + 1 < GetHeight())
                {
                    neighbourList.Add(GetGridObject(currentNode.X - 1, currentNode.Y + 1));
                }
            }
        }
        if (currentNode.X + 1 < GetWidth())
        {
            // Right
            neighbourList.Add(GetGridObject(currentNode.X + 1, currentNode.Y));
            if (canWalkDiagonally)
            {
                // Right Down
                if (currentNode.Y - 1 >= 0)
                {
                    neighbourList.Add(GetGridObject(currentNode.X + 1, currentNode.Y - 1));
                }
                // Right Up
                if (currentNode.Y + 1 < GetHeight())
                {
                    neighbourList.Add(GetGridObject(currentNode.X + 1, currentNode.Y + 1));
                }
            }
        }
        // Bottom
        if (currentNode.Y - 1 >= 0)
        {
            neighbourList.Add(GetGridObject(currentNode.X, currentNode.Y - 1));
        }
        // Top
        if (currentNode.Y + 1 < GetHeight())
        {
            neighbourList.Add(GetGridObject(currentNode.X, currentNode.Y + 1));
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
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
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
        GetXY(worldPosition, out int x, out int y);
        return GetWorldPosition(x, y);
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


    #region TileMap

    public void SetTilemapSprite(int x, int y, GridObject.TilemapSprite tilemapSprite)
    {
        GridObject gridObject = GetGridObject(x, y);
        if (gridObject != null)
        {
            gridObject.SetTilemapSprite(tilemapSprite);
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

    public bool TryMakeBuilding(int startX, int startY, List<BuildingLayer> buildingLayerListFromTopToBottum)
    {
        for (int y = 0; y < buildingLayerListFromTopToBottum.Count; y++)
        {
            for (int x = 0; x < buildingLayerListFromTopToBottum[y].width; x++)
            {
                if (GetGridObject(startX + x, startY + y).State == GridObject.OccupationState.NotPlaceable)
                {
                    return false;
                }
            }
        }

        for (int y = 0; y < buildingLayerListFromTopToBottum.Count; y++)
        {
            for (int x = 0; x < buildingLayerListFromTopToBottum[y].width; x++)
            {
                GetGridObject(startX + x, startY + y).SetOccupationState(buildingLayerListFromTopToBottum[y].occupationState[x]);
            }
        }
        return true;
    }
    public bool TryMakeBuilding(Vector2 worldPosition, List<BuildingLayer> buildingLayerListFromTopToBottum)
    {
        GetXY(worldPosition, out int startX, out int startY);
        return TryMakeBuilding(startX, startY, buildingLayerListFromTopToBottum);
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
            width = width,
            height = height,
            cellSize = cellSize,
            originPosition = originPosition,
            canWalkDiagonally = canWalkDiagonally,
            gridObjectSaveObjectArray = gridObjectSaveObjectList.ToArray(),
        };

        SaveSystem.SaveJson(saveObject, SaveSystem.RootSavePath.DataPath, "Tile Map", "tilemap", false);
    }

    public static Grid Load(string saveObjectJsonData)
    {
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveObjectJsonData);
        
        Grid grid = new Grid(saveObject.width, saveObject.height, saveObject.cellSize, saveObject.originPosition, saveObject.canWalkDiagonally);

        foreach (GridObject.SaveObject gridObjectSaveObject in saveObject.gridObjectSaveObjectArray)
        {
            GridObject gridObject = grid.GetGridObject(gridObjectSaveObject.x, gridObjectSaveObject.y);
            
            gridObject.Load(gridObjectSaveObject);
        }
        grid.OnLoaded?.Invoke(grid, EventArgs.Empty);

        return grid;
    }

    public class SaveObject
    {
        public int width;
        public int height;
        public float cellSize;
        public Vector2 originPosition;
        public bool canWalkDiagonally;
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
