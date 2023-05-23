using Pathfinding;

using TheAshBot;

using UnityEngine;


public class GridManager : MonoBehaviour
{


    public static GridManager Instance
    {
        get;
        private set;
    }


    [Header("Grid")]
    [SerializeField] private GridVisual gridVisual;
    [SerializeField] private TextAsset savedData;

    public Grid grid
    {
        get;
        private set;
    }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GridManager Instance in this scene");
        }
        else
        {
            Instance = this;
        }

        // grid = new Grid(xSize, ySize, cellSize, transform.position, false, true, null);

        grid = Grid.Load(savedData.text);

    }

    private void Start()
    {
        gridVisual.SetGrid(grid);
        AstarPath astarPath = GetComponent<AstarPath>();

        astarPath.AddWorkItem(new AstarWorkItem(ctx =>
        {
            GridGraph gridGraph = AstarPath.active.data.gridGraph;
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                for (int x = 0; x < grid.GetWidth(); x++)
                {
                    if (grid.GetGridObject(x, y).StateList.Contains(GridObject.OccupationState.NotWalkable))
                    {
                        GridNodeBase node = gridGraph.GetNode(x, y);
                        node.Walkable = false;
                    }
                }
            }
        }));
    }

}
