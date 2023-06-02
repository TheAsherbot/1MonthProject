using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{


    public static TeamManager PlayerInstance
    {
        get;
        private set;
    }
    
    public static TeamManager AIInstance
    {
        get;
        private set;
    }


    public event EventHandler<OnMinerialValueChangedEventArgs> OnMinerialValueChanged;
    public class OnMinerialValueChangedEventArgs : EventArgs
    {
        public int changedAmount;
        public int currentValue;
    }


    public enum Teams
    {
        PlayerTeam,
        AITeam,
    }


    [SerializeField] private int startingMinerials;
    [SerializeField] private Teams team;


    private int minerials;

    [Space(5)]

    private int maxNumberOfUnits;
    [SerializeField] private List<_BaseBuilding> teamBuildingList;
    [SerializeField] private List<_BaseUnit> teamUnitList;
    [SerializeField] private List<AttackingUnit> teamAttckingUnitList;


    private void Awake()
    {
        minerials = startingMinerials;

        if (team == Teams.PlayerTeam)
        {
            PlayerInstance = this;
        }
        else if (team == Teams.AITeam)
        {
            AIInstance = this;
        }
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
        if (unit is AttackingUnit attackingUnit)
        {
            teamAttckingUnitList.Add(attackingUnit);
        }
    }
    public void UnitKilled(_BaseUnit unit)
    {
        teamUnitList.Remove(unit);
        if (unit is AttackingUnit attackingUnit)
        {
            teamAttckingUnitList.Remove(attackingUnit);
        }
    }
    public List<_BaseUnit> GetListOfAllUnits()
    {
        return teamUnitList;
    }
    public List<AttackingUnit> GetListOfAllAttackingUnitUnits()
    {
        return teamAttckingUnitList;
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
        List<TownHall> townHallList = new List<TownHall>();
        foreach (_BaseBuilding baseBuilding in teamBuildingList)
        {
            if (baseBuilding is TownHall townHall)
            {
                townHallList.Add(townHall);
            }
        }

        if (townHallList.Count == 0)
        {
            // Trigger End Game
        }
    }
    public List<_BaseBuilding> GetListOfAllBuildings()
    {
        return teamBuildingList;
    }

    #endregion


}
