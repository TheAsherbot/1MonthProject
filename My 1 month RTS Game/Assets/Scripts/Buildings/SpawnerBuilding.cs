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


    [SerializeField] private Transform spawnPosition;


    public void Spawn(Unit spawnedUnit)
    {
        spawnedUnit = Instantiate(spawnedUnit, spawnPosition.position, Quaternion.identity);
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
