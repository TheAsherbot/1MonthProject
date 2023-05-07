using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{


    public event EventHandler<OnMinerialValueChangedEventArgs> OnMinerialValueChanged;
    public class OnMinerialValueChangedEventArgs : EventArgs
    {
        public int changedAmount;
        public int currentValue;
    }


    [SerializeField] private int startingMinerials;


    private int minerials;

    private List<_BaseBuilding> teamBuildingList;
    private List<_BaseUnit> teamUnitList;


    private void Awake()
    {
        teamBuildingList = new List<_BaseBuilding>();
        teamUnitList = new List<_BaseUnit>();

        minerials = startingMinerials;
    }

    private void Start()
    {
        OnMinerialValueChanged?.Invoke(this, new OnMinerialValueChangedEventArgs
        {
            changedAmount = startingMinerials,
            currentValue = minerials
        });
    }


    #region Minerials

    public int GetMinerials()
    {
        return minerials;
    }
    public void AddMinerials(int amount)
    {
        minerials += amount;
        OnMinerialValueChanged?.Invoke(this, new OnMinerialValueChangedEventArgs
        {
            changedAmount = amount,
            currentValue = minerials
        });
    }
    public bool TryUseMinerials(int amount)
    {
        if (minerials - amount < 0) return false;

        minerials -= amount;

        OnMinerialValueChanged?.Invoke(this, new OnMinerialValueChangedEventArgs
        {
            changedAmount = amount,
            currentValue = minerials
        });

        return true;
    }

    #endregion


    #region Unit List

    public void UnitSpawned(_BaseUnit unit)
    {
        teamUnitList.Add(unit);
    }
    public void KillUnit(_BaseUnit unit)
    {
        teamUnitList.Add(unit);
    }

    #endregion


    #region Building List

    public void BuildingSpawned(_BaseBuilding building)
    {
        teamBuildingList.Add(building);
    }
    public void DestoryBuilding(_BaseBuilding building)
    {
        teamBuildingList.Add(building);
    }

    #endregion


}
