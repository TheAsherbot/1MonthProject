using UnityEngine;


public class GridManager : MonoBehaviour
{


    public static GridManager Instance;


    [Header("Grid")]
    [SerializeField] private int xSize = 20;
    [SerializeField] private int ySize = 10;
    [SerializeField] private int cellSize = 10;
    [Space(5)]
    [SerializeField] private GridVisual gridVisual;

    public Grid Grid
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

        Grid = new Grid(xSize, ySize, cellSize, transform.position, false);
    }

    private void Start()
    {
        gridVisual.SetGrid(Grid);
    }

}
