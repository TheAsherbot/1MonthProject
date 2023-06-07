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


    [SerializeField] private int startingMinerials;
    [SerializeField] private Teams team;


    private int minerials;

    [Space(5)]

    [SerializeField] private List<_BaseBuilding> teamBuildingList;
    [SerializeField] private List<_BaseUnit> teamUnitList;
    [SerializeField] private List<Civilian> teamCivilianUnitList;
    [SerializeField] private List<AttackingUnit> teamAttckingUnitList;


    [Header("Feeding")]
    [SerializeField] private int costToFeed = 10;
    [SerializeField] private float feedTimerMax = 60;
    private float feedTimer = 0;


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

    private void Update()
    {
        feedTimer += Time.deltaTime;

        if (feedTimer > feedTimerMax)
        {
            feedTimer = 0;

            foreach (Civilian civilian in teamCivilianUnitList)
            {
                if (minerials >= costToFeed)
                {
                    AddMinerials(-costToFeed);
                }
                else
                {
                    civilian.Starve();
                }
            }
            foreach (AttackingUnit attackingUnit in teamAttckingUnitList)
            {
                if (minerials >= costToFeed)
                {
                    minerials -= costToFeed;
                }
                else
                {
                    attackingUnit.Starve();
                }
            }
        }
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

        if (unit is Civilian civilian)
        {
            teamCivilianUnitList.Add(civilian);
        }
        if (unit is AttackingUnit attackingUnit)
        {
            teamAttckingUnitList.Add(attackingUnit);
        }
    }
    public void UnitKilled(_BaseUnit unit)
    {
        teamUnitList.Remove(unit);

        if (teamUnitList.Count == 0 && minerials < 50)
        {
            GameManager.Instance.GameOver(team);
        }

        if (unit is Civilian civilian)
        {
            teamCivilianUnitList.Remove(civilian);
        }
        if (unit is AttackingUnit attackingUnit)
        {
            teamAttckingUnitList.Remove(attackingUnit);
        }
    }
    public List<_BaseUnit> GetListOfAllUnits()
    {
        return teamUnitList;
    }
    public List<Civilian> GetListOfAllCivilianUnits()
    {
        return teamCivilianUnitList;
    }
    public List<AttackingUnit> GetListOfAllAttackingUnits()
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
        teamBuildingList.Remove(building);
        
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
            GameManager.Instance.GameOver(team);
        }
    }
    public List<_BaseBuilding> GetListOfAllBuildings()
    {
        return teamBuildingList;
    }

    #endregion


}
