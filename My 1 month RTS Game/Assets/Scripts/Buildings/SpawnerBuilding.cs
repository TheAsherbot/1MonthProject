using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBuilding : _BaseBuilding, ISelectable
{


    public bool IsSelected
    {
        get;
        set;
    }


    [SerializeField] private List<UnitSO> spawnAbleUnits;
    [SerializeField] private Transform spawnPosition;


    private void Update()
    {
        if (IsSelected)
        {
            HandelInput();
        }
    }


    private void HandelInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Spawn(spawnAbleUnits[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //Spawn(spawnAbleUnits[1].prefab);
        }
    }

    private void Spawn(UnitSO unitSO)
    {
        _BaseUnit spawnedUnit = Instantiate(unitSO.prefab, spawnPosition.position, Quaternion.identity);
        spawnedUnit.name = unitSO.name;
    }

    public void Select()
    {
        IsSelected = true;
    }

    public void Unselect()
    {
        IsSelected = false;
    }
}
