using System.Collections.Generic;

using UnityEngine;

public class TownHall : _BaseBuilding, ISelectable
{


    #region Variables

    public bool IsSelected
    {
        get;
        set;
    }
    [field : SerializeField] 
    public List<HotBarSlotSO> HotBarSlotSOList
    {
        get;
        set;
    }

    [SerializeField] private List<UnitSO> spawnableUnits;
    [SerializeField] private Transform unloadTransform;
    [SerializeField] private Transform loadTransform;
    [Space(5)]
    [SerializeField] private TeamManager teamManager;

    #endregion


    #region Unity Functions

    private void Start()
    {
        GridManager.Instance.grid.TryMakeBuilding(transform.position, buildingSO.buildingLeyerListFromBottomToTop);
    }

    private void Update()
    {
        if (IsSelected)
        {
            TestInput();
        }
    }

    #endregion


    #region public

    public Transform GetLoadTransform()
    {
        return loadTransform;
    }

    public TeamManager GetTeamManager()
    {
        return teamManager;
    }

    public void AddMinerials()
    {
        int amount = 10;
        teamManager.AddMinerials(amount);
    }

    #endregion


    #region Input

    private void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Spawn(spawnableUnits[0]);
        }
    }

    private void Spawn(UnitSO unitSO)
    {
        if (teamManager.TryUseMinerials(unitSO.cost))
        {
            _BaseUnit spawnedUnit = Instantiate(unitSO.prefab, unloadTransform.position, Quaternion.identity);
            spawnedUnit.name = unitSO.name;
            teamManager.UnitSpawned(spawnedUnit);
            if (spawnedUnit is Civilian)
            {
                ((Civilian)spawnedUnit).SetTownHall(this);
            }
        }
    }

    #endregion


    #region Interfaces

    public void Select()
    {
        IsSelected = true;
    }

    public void Unselect()
    {
        IsSelected = false;
    }

    public void OnSlot1ButtonClicked()
    {
        Spawn(spawnableUnits[0]);
    }

    public void OnSlot2ButtonClicked()
    {
        Spawn(spawnableUnits[1]);
    }

    public void OnSlot3ButtonClicked()
    {
        Spawn(spawnableUnits[2]);
    }

    public void OnSlot4ButtonClicked()
    {
        Spawn(spawnableUnits[3]);
    }

    public void OnSlot5ButtonClicked()
    {
        Spawn(spawnableUnits[4]);
    }

    #endregion


}
