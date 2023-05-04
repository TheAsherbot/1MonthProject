using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class BuildingManager : MonoBehaviour
{


    [SerializeField] private List<BuildingSO> buildingSOList;


    private BuildingSO selectedBuilding;
    private Grid grid;


    private void Start()
    {
        grid = GridManager.Instance.Grid;
    }

    private void Update()
    {
        TestInput();
    }


    private void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBuilding = buildingSOList[0];
        }

        if (Input.GetMouseButtonDown(2))
        {
            PlaceBuilding();
        }
    }

    private void PlaceBuilding()
    {
        Vector2 mouseWorldPosition = Mouse2D.GetMousePosition2D();
        if (selectedBuilding != null)
        {
            if (grid.TryMakeBuilding(mouseWorldPosition, selectedBuilding.buildingWidthListFromTopToBottom))
            {
                GameObject newGameObject = Instantiate(buildingSOList[0].prefab, grid.SnapPositionToGrid(mouseWorldPosition), Quaternion.identity);
                newGameObject.name = selectedBuilding.name;
            }
        }
    }

}
