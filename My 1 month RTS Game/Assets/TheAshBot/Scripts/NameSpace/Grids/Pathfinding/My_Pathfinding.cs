using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public class My_Pathfinding
{
    
    
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


    private int width;
    private int height;
    private float cellSize;
    private Vector2 originPosition;
    private PathNode[,] gridArray;

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

    private List<PathNode> openList;
    private List<PathNode> closedList;


    public My_Pathfinding(int width, int height, float cellSize, Vector2 originPosition, bool showDebug, Transform parent)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new PathNode[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new PathNode(this, x, y);
            }
        }

        // Doing it agian becouse now all fo the path nodes have been made.
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                GetPathNode(x, y).neighbourNodeList = GetNeighbourList(GetPathNode(x, y));
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
                }
            }

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
    }
    public My_Pathfinding(int width, int height, float cellSize, Vector2 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new PathNode[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new PathNode(this, x, y);
            }
        }

        // Doing it agian becouse now all fo the path nodes have been made.
        for (int x = 0; x < GetWidth(); x++)
        {
            for (int y = 0; y < GetHeight(); y++)
            {
                GetPathNode(x, y).neighbourNodeList = GetNeighbourList(GetPathNode(x, y));
            }
        }
    }

    /// <summary>
    /// This will find the path from point A on the grid to point B on the grid
    /// </summary>
    /// <param name="startX">This is the X poisition on the grid of the start node</param>
    /// <param name="startY">This is the Y poisition on the grid of the start node</param>
    /// <param name="endX">This is the X position on the grid of the end node</param>
    /// <param name="endY">This is the Y position on the grid of the end node</param>
    /// <returns>Returns the path as a list of Vector2s, starting at the start node; ending at the end node</returns>
    public List<Vector2> FindPathAsVector2s(int startX, int startY, int endX, int endY)
    {
        List<PathNode> path = FindPathAsPathNodes(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector2> vectorPath = new List<Vector2>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector2(pathNode.x, pathNode.y) * GetCellSize() + Vector2.one * GetCellSize() * 0.5f + originPosition);
            }
            return vectorPath;
        }
    }
    /// <summary>
    /// This will find the path from point A on the grid to point B on the grid
    /// </summary>
    /// <param name="startWorldPosition">This is the world poisition on the grid of the start node</param>
    /// <param name="endX">This is the X position on the grid of the end node</param>
    /// <param name="endY">This is the Y position on the grid of the end node</param>
    /// <returns>Returns the path as a list of Vector2s, starting at the start node; ending at the end node</returns>
    public List<Vector2> FindPathAsVector2s(Vector2 startWorldPosition, int endX, int endY)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        return FindPathAsVector2s(startX, startY, endX, endY);
    }
    /// <summary>
    /// This will find the path from point A on the grid to point B on the grid
    /// </summary>
    /// <param name="startWorldPosition">This is the world poisition on the grid of the start node</param>
    /// <param name="endWorldPosition">This is the world poisition on the grid of the emd node</param>
    /// <returns>Returns the path as a list of Vector2s, starting at the start node; ending at the end node</returns>
    public List<Vector2> FindPathAsVector2s(Vector2 startWorldPosition, Vector2 endWorldPosition)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsVector2s(startX, startY, endX, endY);
    }
    /// <summary>
    /// This will find the path from point A on the grid to point B on the grid
    /// </summary>
    /// <param name="startX">This is the X poisition on the grid of the start node</param>
    /// <param name="startY">This is the Y poisition on the grid of the start node</param>
    /// <param name="endWorldPosition">This is the world poisition on the grid of the emd node</param>
    /// <returns>Returns the path as a list of Vector2s, starting at the start node; ending at the end node</returns>
    public List<Vector2> FindPathAsVector2s(int startX, int startY, Vector2 endWorldPosition)
    {
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsVector2s(startX, startY, endX, endY);
    }


    /// <summary>
    /// This will find the path from point A on the grid to point B on the grid
    /// </summary>
    /// <param name="startX">This is the X poisition on the grid of the start node</param>
    /// <param name="startY">This is the Y poisition on the grid of the start node</param>
    /// <param name="endX">This is the X position on the grid of the end node</param>
    /// <param name="endY">This is the Y position on the grid of the end node</param>
    /// <returns>Returns the path as a list of Path Nodes, starting at the start node; ending at the end node</returns>
    public List<PathNode> FindPathAsPathNodes(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = GetPathNode(startX, startY);
        PathNode endNode = GetPathNode(endX, endY);

        if (endNode == null)
        {
            // End node is not on the grid
            return default;
        }

        openList = new List<PathNode>() { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < GetWidth(); x++)
        {
            for (int y = 0; y < GetHeight(); y++)
            {
                PathNode pathNode = GetPathNode(x, y);
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
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached finel node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
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
    /// <summary>
    /// This will find the path from point A on the grid to point B on the grid
    /// </summary>
    /// <param name="startWorldPosition">This is the world poisition on the grid of the start node</param>
    /// <param name="endX">This is the X position on the grid of the end node</param>
    /// <param name="endY">This is the Y position on the grid of the end node</param>
    /// <returns>Returns the path as a list of Path Nodes, starting at the start node; ending at the end node</returns>
    public List<PathNode> FindPathAsPathNodes(Vector2 startWorldPosition, int endX, int endY)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        return FindPathAsPathNodes(startX, startY, endX, endY);
    }
    /// <summary>
    /// This will find the path from point A on the grid to point B on the grid
    /// </summary>
    /// <param name="startWorldPosition">This is the world poisition on the grid of the start node</param>
    /// <param name="endWorldPosition">This is the world poisition on the grid of the emd node</param>
    /// <returns>Returns the path as a list of Path Nodes, starting at the start node; ending at the end node</returns>
    public List<PathNode> FindPathAsPathNodes(Vector2 startWorldPosition, Vector2 endWorldPosition)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsPathNodes(startX, startY, endX, endY);
    }
    /// <summary>
    /// This will find the path from point A on the grid to point B on the grid
    /// </summary>
    /// <param name="startX">This is the X poisition on the grid of the start node</param>
    /// <param name="startY">This is the Y poisition on the grid of the start node</param>
    /// <param name="endWorldPosition">This is the world poisition on the grid of the emd node</param>
    /// <returns>Returns the path as a list of Path Nodes, starting at the start node; ending at the end node</returns>
    public List<PathNode> FindPathAsPathNodes(int startX, int startY, Vector2 endWorldPosition)
    {
        GetXY(endWorldPosition, out int endX, out int endY);
        return FindPathAsPathNodes(startX, startY, endX, endY);
    }


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
    /// THis sets the value of a cell using it's 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetPathNode(int x, int y, PathNode value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (value == null)
            {
                value = default;
            }

            gridArray[x, y] = value;

            TriggerPathNodeChanged(x, y);
        }
    }

    /// <summary>
    /// This sets the vaue of a cell using it's world position
    /// </summary>
    /// <param name="worldPosition">This is the grid objects world position<</param>
    /// <param name="value">This is the value it is being set to</param>
    public void SetPathNode(Vector2 worldPosition, PathNode value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetPathNode(x, y, value);
    }

    /// <summary>
    /// This gets the value of a cell using it's positon on the grid
    /// </summary>
    /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
    /// <param name="y">This is the number of grid objects above the start grid object</param>
    /// <returns>Returns the grid object</returns>
    public PathNode GetPathNode(int x, int y)
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
    public PathNode GetPathNode(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetPathNode(x, y);
    }

    public void TriggerPathNodeChanged(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs
        {
            x = x,
            y = y
        });
    }

    /// <summary>
    /// This will get all the neighbours of a spicifec node NOTE: in this code I cash them when the grid is created so please use pathNode.neighbourNodeList
    /// </summary>
    /// <param name="currentNode">This is the node that you are getting hte neighbours of</param>
    /// <returns>the list of neighbours</returns>
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetPathNode(currentNode.x -1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetPathNode(currentNode.x - 1, currentNode.y - 1));
            }
            // Left Up
            if (currentNode.y + 1 < GetHeight())
            {
                neighbourList.Add(GetPathNode(currentNode.x - 1, currentNode.y + 1));
            }
        }
        if (currentNode.x + 1 < GetWidth())
        {
            // Right
            neighbourList.Add(GetPathNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetPathNode(currentNode.x + 1, currentNode.y - 1));
            }
            // Right Up
            if (currentNode.y + 1 < GetHeight())
            {
                neighbourList.Add(GetPathNode(currentNode.x + 1, currentNode.y + 1));
            }
        }
        // Bottom
        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(GetPathNode(currentNode.x, currentNode.y - 1));
        }
        // Top
        if (currentNode.y + 1 < GetHeight())
        {
            neighbourList.Add(GetPathNode(currentNode.x, currentNode.y + 1));
        }

        return neighbourList;
    }

    /// <summary>
    /// This get the path from the end using the "cameFromNode" variable
    /// </summary>
    /// <param name="endNode">this is the end of the path</param>
    /// <returns>a list of pathnodes srating at the start point; ending at the end point</returns>
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List <PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    /// <summary>
    /// This calculates the absolute value of the distance betwwen two nodes.
    /// </summary>
    /// <param name="a">This is the first node</param>
    /// <param name="b">This is the last node</param>
    /// <returns>the distance between the nodes as a floast</returns>
    private float CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return moveDiagonalCost * Mathf.Min(xDistance, yDistance) + moveStraightCost * remaining;
    }

    /// <summary>
    /// This calculates the lowest F cost from a lists of PathNodes
    /// </summary>
    /// <param name="pathNodeList">This is the path nodes that it searches (Mosty used with the Open List)</param>
    /// <returns>the Path Node with the lowest F Cost</returns>
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) 
    {
        PathNode lowestFCostNode = pathNodeList[0];
        
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
    
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
