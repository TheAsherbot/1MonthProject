using System;
using System.Collections.Generic;

using UnityEngine;

namespace TheAshBot.Grid
{
    public class IntHexGrid
    {


        private const float HEX_VERTICAL_OFFSET_MULTIPLIER = 0.75f;


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
        private int[,] gridArray;


        /// <summary>
        /// This makes a grid that each cell holds a boolean value
        /// </summary>
        /// <param name="width">This is the width of the grid</param>
        /// <param name="height">THis is the hight of the grid</param>
        /// <param name="cellSize">This is how big the grid objects are</param>
        /// <param name="originPosition">This is the position of the bottum left grid object(AKA the origin</param>
        /// <param name="showDebug">If this is true the it will show the lines of the grid</param>
        /// <param name="parent">This si the parent object of the text(This is only needed if show debug is true)</param>
        public IntHexGrid(int width, int height, float cellSize, Vector2 originPosition, bool showDebug, Transform parent)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new int[width, height];

            if (showDebug)
            {
                TextMesh[,] debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        debugTextArray[x, y] = CreateWorldText(parent, gridArray[x, y].ToString(), GetWorldPosition(x, y), 5 * (int)cellSize, Color.white, TextAnchor.MiddleCenter);
                    }
                }

                OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
                {
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
                };
            }
        }

        /// <summary>
        /// This makes a grid that each cell holds a boolean value
        /// </summary>
        /// <param name="width">This is the width of the grid</param>
        /// <param name="height">THis is the hight of the grid</param>
        /// <param name="cellSize">This is how big the grid objects are</param>
        /// <param name="originPosition">This is the position of the bottum left grid object(AKA the origin</param>
        public IntHexGrid(int width, int height, float cellSize, Vector2 originPosition)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new int[width, height];
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
            return 
                new Vector2(x, 0) * cellSize + 
                new Vector2(0, y) * cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER + 
                ((y % 2) == 1 ? new Vector2(1, 0) * cellSize * 0.5f : Vector2.zero) + 
                originPosition;
        }

        /// <summary>
        /// This gets the x and y position of a grid object using it's world position
        /// </summary>
        /// <param name="worldPosition">This is the world position of the grid object</param>
        /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
        /// <param name="y">This is the number of grid objects above the start grid object</param>
        public void GetXY(Vector2 worldPosition, out int x, out int y)
        {
            int roughX = Mathf.RoundToInt((worldPosition - originPosition).x / cellSize);
            int roughY = Mathf.RoundToInt((worldPosition - originPosition).y / cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER);

            Vector2Int roughXY = new Vector2Int(roughX, roughY);

            bool isOddRow = roughY % 2 == 1;

            List<Vector2Int> neighbourXYList = new List<Vector2Int>
            {
                roughXY + new Vector2Int(-1, 0),
                roughXY + new Vector2Int(+1, 0),
                 
                roughXY + new Vector2Int(isOddRow ? + 1: - 1, +1),
                roughXY + new Vector2Int(+0, +1),

                roughXY + new Vector2Int(isOddRow ? + 1: - 1 , -1),
                roughXY + new Vector2Int(+0, -1),
            };

            Vector2Int closestXY = roughXY;

            foreach (Vector2Int neighbourXY in neighbourXYList)
            { 
                if (Vector2.Distance(worldPosition, GetWorldPosition(neighbourXY.x, neighbourXY.y)) < Vector2.Distance(worldPosition, GetWorldPosition(closestXY.x, closestXY.y)))
                {
                    // neighbourXY is closer then closestXY
                    closestXY = neighbourXY;
                }
            }

            x = closestXY.x;
            y = closestXY.y;
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
        public void SetValue(int x, int y, int value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {

                gridArray[x, y] = value;
                if (OnGridValueChanged != null)
                {
                    OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
                }
            }
        }

        /// <summary>
        /// This sets the vaue of a cell using it's world position
        /// </summary>
        /// <param name="worldPosition">This is the grid objects world position<</param>
        /// <param name="value">This is the value it is being set to</param>
        public void SetValue(Vector2 worldPosition, int value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetValue(x, y, value);
        }

        /// <summary>
        /// This gets the value of a cell using it's positon on the grid
        /// </summary>
        /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
        /// <param name="y">This is the number of grid objects above the start grid object</param>
        /// <returns>Returns the grid object</returns>
        public int GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// This gets the value of a cell using it's world position
        /// </summary>
        /// <param name="worldPosition">This is the grid objects world position</param>
        /// <returns>This ruterns the grid object</returns>
        public int GetValue(Vector2 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetValue(x, y);
        }

        /// <summary>
        /// This adds the to the value of a cell using it's positon on the grid
        /// </summary>
        /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
        /// <param name="y">This is the number of grid objects above the start grid object</param>
        /// <param name="value">This is the value being adding to the previus value</param>
        public void AddValue(int x, int y, int value)
        {
            SetValue(x, y, GetValue(x, y) + value);
        }

        /// <summary>
        /// This adds the to the value of a cell using it's world position
        /// </summary>
        /// <param name="worldPosition">This is the grid objects world position</param>
        /// <param name="value">This is the value being adding to the previus value</param>
        public void AddValue(Vector2 worldPosition, int value)
        {
            SetValue(worldPosition, GetValue(worldPosition) + value);
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
}
