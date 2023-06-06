using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour, ISelectable
{


    public bool IsSelected
    {
        get;
        set;
    }
    public List<HotBarSlotSO> HotBarSlotSOList
    {
        get;
        set;
    }


    [SerializeField] private List<BuildingSO> buildingSOList;


    private Grid grid;
    private BuildingSO selectedBuilding;
    private GameInputActions inputActions;



    #region Unity Functions

    private void Awake()
    {
        inputActions = new GameInputActions();
        inputActions.Game.Enable();
    }

    private void Start()
    {
        grid = GridManager.Instance.grid;
    }

    #endregion


    #region PrivateFunctions

    private void PlaceBuilding()
    {
        Vector2 mouseWorldPosition = Mouse2D.GetMousePosition2D();
        if (selectedBuilding != null)
        {
            if (grid.TryMakeBuilding(mouseWorldPosition, selectedBuilding.buildingLeyerListFromBottomToTop))
            {
                _BaseBuilding newBuilding = Instantiate(selectedBuilding.prefab, grid.SnapPositionToGrid(mouseWorldPosition), Quaternion.identity);
                newBuilding.gameObject.name = selectedBuilding.name;
            }
        }
    }

    #endregion


    public void OnSlot1ButtonClicked()
    {
        selectedBuilding = buildingSOList[0];
    }

    public void OnSlot2ButtonClicked()
    {
        selectedBuilding = buildingSOList[1];
    }

    public void OnSlot3ButtonClicked()
    {
        selectedBuilding = buildingSOList[2];
    }
}
