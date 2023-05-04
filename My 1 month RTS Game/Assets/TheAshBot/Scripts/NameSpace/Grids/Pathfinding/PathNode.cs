using System.Collections.Generic;

public class PathNode
{


    public float gCost;
    public float hCost;
    public float fCost;

    public int x;
    public int y;

    public bool isWalkable; 
    public PathNode cameFromNode;


    private Pathfinding grid;

    public List<PathNode> neighbourNodeList;
     
    public PathNode(Pathfinding grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerPathNodeChanged(x, y);
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
