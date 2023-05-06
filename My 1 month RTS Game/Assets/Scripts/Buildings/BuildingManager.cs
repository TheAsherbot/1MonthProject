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
        grid = GridManager.Instance.grid;
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
            if (grid.TryMakeBuilding(mouseWorldPosition, selectedBuilding.buildingLeyerListFromBottomToTop))
            {
                _BaseBuilding newBuilding = Instantiate(buildingSOList[0].prefab, grid.SnapPositionToGrid(mouseWorldPosition), Quaternion.identity);
                newBuilding.gameObject.name = selectedBuilding.name;
            }
        }
    }

}
