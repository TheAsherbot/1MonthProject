using System.Collections;
using System.Collections.Generic;

using TheAshBot.Grid;

using UnityEngine;

public class PathNode
{


    public int gCost;
    public int hCost;
    public int fCost;

    public int x;
    public int y;

    public bool isWalkable; 
    public PathNode cameFromNode;


    private GenericGrid<PathNode> grid;

     
    public PathNode(GenericGrid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        if (isWalkable)
        {
            return "T";
        }
        else
        {
            return "F";
        }
    }


}
