using System.Collections.Generic;

using UnityEngine;

public class TownHall : _BaseBuilding, ISelectable, IDamageable
{


    #region Variables

    [SerializeField] private GameObject selectedVisual;

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
    
    
    private TeamManager teamManager;


    [Header("Health")]
    [SerializeField] private int maxHealth;
    private HealthSystem healthSystem;

    #endregion


    #region Unity Functions

    private void Start()
    {
        Vector3 offset = new Vector3(1.6f, 2.5f);
        Vector3 size = new Vector3(3, 0.3f);
        HealthBar.Border border = new HealthBar.Border();
        border.thickness = 0.075f;
        border.color = Color.black;
        healthSystem = HealthBar.Create(maxHealth, transform, offset, size, Color.red, Color.gray, border, false, 13);
        GridManager.Instance.grid.TryMakeBuilding(transform.position, buildingSO.buildingLeyerListFromBottomToTop);
        if (GetComponent<IsOnPlayerTeam>() != null)
        {
            // Is On Player team
            teamManager = TeamManager.PlayerInstance;
        }
        else
        {
            // is not on player team (So is on AI team)
            teamManager = TeamManager.AIInstance;
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

    private void Spawn(UnitSO unitSO)
    {
        Vector2 raycastPosition = (Vector2)transform.position + new Vector2(3.5f, 1.5f);
        RaycastHit2D raycastHit = Physics2D.Raycast(raycastPosition, Vector3.forward, 100f);

        if (raycastHit.transform != null)
        {
            return; // Unit is alrady at the spawn point
        }

        if (teamManager.TryUseMinerials(unitSO.cost))
        {
            _BaseUnit spawnedUnit = Instantiate(unitSO.prefab, unloadTransform.position, Quaternion.identity);
            spawnedUnit.name = unitSO.name;
            teamManager.UnitSpawned(spawnedUnit);
            if (GetComponent<IsOnPlayerTeam>() != null)
            {
                // Is on player team
                spawnedUnit.gameObject.AddComponent<IsOnPlayerTeam>();
            }
            else
            {
                // is on AI team
                spawnedUnit.gameObject.AddComponent<IsOnAITeam>();
            }
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

        selectedVisual.SetActive(true);
    }

    public void Unselect()
    {
        IsSelected = false;

        selectedVisual.SetActive(false);
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




    public void Damage(int amount)
    {
        healthSystem.Damage(amount);
    }

    #endregion


}
