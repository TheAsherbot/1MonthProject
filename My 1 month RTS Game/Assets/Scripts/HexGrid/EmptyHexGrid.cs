using System;
using System.Collections.Generic;

using UnityEngine;

namespace TheAshBot.Grid
{
    public class EmptyHexGrid
    {


        private const float HEX_VERTICAL_OFFSET_MULTIPLIER = 0.75f;


        private int width;
        private int height;
        private float cellSize;
        private Vector2 originPosition;


        /// <summary>
        /// This makes a grid that each cell holds a boolean value
        /// </summary>
        /// <param name="width">This is the width of the grid</param>
        /// <param name="height">THis is the hight of the grid</param>
        /// <param name="cellSize">This is how big the grid objects are</param>
        /// <param name="originPosition">This is the position of the bottum left grid object(AKA the origin</param>
        public EmptyHexGrid(int width, int height, float cellSize, Vector2 originPosition)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
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
            bool isOddRow = worldPosition.y % 2 == 1;

            Vector2Int worldPositionAsVector2Int = new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));

            List<Vector2Int> neighbourXYList = new List<Vector2Int>
            {
                worldPositionAsVector2Int + new Vector2Int(-1, 0),
                worldPositionAsVector2Int + new Vector2Int(+1, 0),

                worldPositionAsVector2Int + new Vector2Int(isOddRow ? + 1: - 1, +1),
                worldPositionAsVector2Int + new Vector2Int(+0, +1),

                worldPositionAsVector2Int + new Vector2Int(isOddRow ? + 1: - 1 , -1),
                worldPositionAsVector2Int + new Vector2Int(+0, -1),
            };

            Vector2Int closestXY = worldPositionAsVector2Int;

            foreach (Vector2Int neighbourXY in neighbourXYList)
            {
                if (Vector2.Distance(worldPosition, GetWorldPosition(neighbourXY.x, neighbourXY.y)) < Vector2.Distance(worldPosition, GetWorldPosition(closestXY.x, closestXY.y)))
                {
                    // neighbourXY is closer then closestXY
                    closestXY = neighbourXY;
                }
            }

            return new Vector2(closestXY.x, closestXY.y);
        }

    }
}
