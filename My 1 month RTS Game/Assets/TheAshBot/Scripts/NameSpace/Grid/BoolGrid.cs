using System;
using UnityEngine;

namespace TheAshBot.Grid
{
    public class BoolGrid
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
        private bool[,] gridArray;

        /// <summary>
        /// This makes a grid that each cell holds a boolean value
        /// </summary>
        /// <param name="width">This is the width of the grid</param>
        /// <param name="height">THis is the hight of the grid</param>
        /// <param name="cellSize">This is how big the grid objects are</param>
        /// <param name="originPosition">This is the position of the bottum left grid object(AKA the origin</param>
        /// <param name="showDebug">If this is true the it will show the lines of the grid</param>
        /// <param name="parent">This si the parent object of the text(This is only needed if show debug is true)</param>
        public BoolGrid(int width, int height, float cellSize, Vector2 originPosition, bool showDebug, Transform parent)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new bool[width, height];

            if (parent == null)
            {
                showDebug = false;
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

        /// <summary>
        /// This makes a grid that each cell holds a boolean value
        /// </summary>
        /// <param name="width">This is the width of the grid</param>
        /// <param name="height">THis is the hight of the grid</param>
        /// <param name="cellSize">This is how big the grid objects are</param>
        /// <param name="originPosition">This is the position of the bottum left grid object(AKA the origin</param>
        public BoolGrid(int width, int height, float cellSize, Vector2 originPosition)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new bool[width, height];
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
            int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
            return new Vector2(x, y);
        }

        /// <summary>
        /// THis sets the value of a cell using it's 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public void SetValue(int x, int y, bool value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            }
        }

        /// <summary>
        /// This sets the vaue of a cell using it's world position
        /// </summary>
        /// <param name="worldPosition">This is the grid objects world position<</param>
        /// <param name="value">This is the value it is being set to</param>
        public void SetValue(Vector2 worldPosition, bool value)
        {
            int x;
            int y;
            GetXY(worldPosition, out x, out y);
            SetValue(x, y, value);
        }

        /// <summary>
        /// This sets the value of a call to the oposite value using it's position on the grid
        /// </summary>
        /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
        /// <param name="y">This is the number of grid objects above the start grid object</param>
        public void SetOppositeValue(int x, int y)
        {
            bool value = !GetValue(x, y);
            SetValue(x, y, value);
        }

        /// <summary>
        /// This sets the value of a call to the oposite value using it's world position
        /// </summary>
        /// <param name="worldPosition">This is the grid objects world position</param>
        public void SetOppositeValue(Vector2 worldPosition)
        {
            int x;
            int y;
            GetXY(worldPosition, out x, out y);
            SetOppositeValue(x, y);
        }

        /// <summary>
        /// This gets the value of a cell using it's positon on the grid
        /// </summary>
        /// <param name="x">This is the number of grid objects to the right of the start grid object</param>
        /// <param name="y">This is the number of grid objects above the start grid object</param>
        /// <returns>Returns the grid object</returns>
        public bool GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This gets the value of a cell using it's world position
        /// </summary>
        /// <param name="worldPosition">This is the grid objects world position</param>
        /// <returns>This ruterns the grid object</returns>
        public bool GetValue(Vector2 worldPosition)
        {
            int x;
            int y;
            GetXY(worldPosition, out x, out y);
            return GetValue(x, y);
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
