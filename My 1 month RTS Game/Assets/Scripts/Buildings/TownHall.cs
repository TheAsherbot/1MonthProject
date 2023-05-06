using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHall : _BaseBuilding, ISelectable
{


    public bool IsSelected
    {
        get;
        set;
    }


    [SerializeField] private List<UnitSO> spawnAbleUnits;
    [SerializeField] private Transform unloadTransform;
    [SerializeField] private Transform loadTransform;


    private void Update()
    {
        if (IsSelected)
        {
            TestInput();
        }
    }


    public Transform GetLoadTransform() 
    {
        return loadTransform;
    }

    private void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Spawn(spawnAbleUnits[0]);
        }
    }

    private void Spawn(UnitSO unitSO)
    {
        _BaseUnit spawnedUnit = Instantiate(unitSO.prefab, unloadTransform.position, Quaternion.identity);
        spawnedUnit.name = unitSO.name;
        if (spawnedUnit is Civilian)
        {
            ((Civilian)spawnedUnit).SetTownHall(this);
        }

    }

    

    #region Interfaces

    public void Select()
    {
        IsSelected = true;
    }

    public void Unselect()
    {
        IsSelected = false;
    }

    #endregion

}
