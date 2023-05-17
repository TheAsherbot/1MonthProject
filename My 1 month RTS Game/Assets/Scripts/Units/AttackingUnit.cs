using System;
using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;
using UnityEngine.InputSystem;

public class AttackingUnit : _BaseUnit, ISelectable, IDamageable
{


    public enum States
    {
        Idle,
        Moving,
        MovingToAttack,
        Attacking,
    }


    #region Events

    public event EventHandler<OnAttackEventArgs> OnAttack;
    public class OnAttackEventArgs : EventArgs
    {
        public Transform enemy;
    }

    #endregion


    #region Variables

    [Header("Visual")]
    [SerializeField] private GameObject selectedVisual;
    [field: SerializeField]
    public List<HotBarSlotSO> HotBarSlotSOList
    {
        get;
        set;
    }


    public bool IsSelected
    {
        get;
        set;
    }
    private States state;

    
    [Header("Health")]
    [SerializeField] private int maxHealth;
    private HealthSystem healthSystem;

    [Header("Upgrades")]
    [SerializeField] private int maxNumberOfUpgrades = 3;
    private int numberOfUpgrades = 0;
    private TeamManager teamManager;


    [Header("Attacking")]
    [SerializeField] private bool useExternailDamageCode = false;
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float startAttackingRange = 5f;
    [SerializeField] private float timeToAttack;
    [SerializeField] private int damage;

    private float attack_ElapsedTime;
    private IDamageable iDamageableEnemy;
    private _BaseIsOnTeam enemy;
    private Vector2 last_EnemyPosition;


    [Header("Input")]
    private GameInputActions inputActions;

    #endregion

    
    #region Unity Functions

    private new void Start()
    {
        base.Start();

        Vector2 healthBarOffset = Vector3.up * 2;
        Vector2 healthBarSize = new Vector3(3, 0.3f);
        healthSystem = HealthBar.Create(maxHealth, transform, healthBarOffset, healthBarSize, Color.red, Color.gray, new HealthBar.Border { color = Color.black, thickness = .1f });

        inputActions = new GameInputActions();
        inputActions.Game.Enable();
        inputActions.Game.Action1.performed += Action1_performed;

        attack_ElapsedTime = timeToAttack;
        OnReachedDestination += SwordsMan_OnReachedDestination;

        healthSystem.OnHealthDepleted += HealthSystem_OnHealthDepleted;

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

    private void Update()
    {
        TestState();
    }

    #endregion


    #region pubic

    public Vector2 GetDirectionToEnemy()
    {
        if (enemy == null)
        {
            return last_EnemyPosition - (Vector2)transform.position;
        }
        Vector2 direction = (Vector2)enemy.transform.position - (Vector2)transform.position;
        return direction;
    }

    public int GetDamageAmount()
    {
        return damage;
    }

    #endregion


    #region Input

    private void Action1_performed(InputAction.CallbackContext obj)
    {
        if (IsSelected)
        {
            MoveInputPressed();
        }
    }

    private void MoveInputPressed()
    {
        if (!GridManager.Instance.grid.IsPositionOnGrid(Mouse2D.GetMousePosition2D())) return;

        state = States.Moving;
        if (Mouse2D.TryGetObjectAtMousePosition(out GameObject hit))
        {
            if (gameObject.TryGetComponent<IsOnPlayerTeam>(out var trash)) // I do not need the "trash" variable
            {
                if (hit.TryGetComponent(out IsOnAITeam enemy))
                {
                    enemy.TryGetComponent(out iDamageableEnemy);
                    this.enemy = enemy;
                    state = States.MovingToAttack;
                }
            }
            else
            {
                if (hit.TryGetComponent(out IsOnPlayerTeam enemy))
                {
                    enemy.TryGetComponent(out iDamageableEnemy);
                    this.enemy = enemy;
                    state = States.MovingToAttack;
                }
            }
            
        }

        Trigger_OnMove(Mouse2D.GetMousePosition2D());
    }

    #endregion


    #region State

    private void TestState()
    {
        switch (state)
        {
            case States.Idle:
                break;
            case States.Moving:
                break;
            case States.MovingToAttack:
                GoingToAttackState();
                break;
            case States.Attacking:
                AttackingState();
                break;
            default:
                state = States.Idle;
                break;
        }
    }

    private void GoingToAttackState()
    {
        if (GridManager.Instance.grid.SnapPositionToGrid(enemy.transform.position) != GridManager.Instance.grid.SnapPositionToGrid(last_EnemyPosition))
        {
            last_EnemyPosition = GridManager.Instance.grid.SnapPositionToGrid(enemy.transform.position);
            Trigger_OnMove(enemy.transform.position);
        }

        if (Vector2.Distance(transform.position, enemy.transform.position) <= startAttackingRange)
        {
            Trigger_OnStopMoveing();
            state = States.Attacking;
        }
    }

    private void AttackingState()
    {
        if (iDamageableEnemy == null || enemy == null)
        {
            state = States.Idle;
            return;
        }

        if (Vector2.Distance(transform.position, enemy.transform.position) > startAttackingRange)
        {
            state = States.MovingToAttack;

            Trigger_OnMove(enemy.transform.position);
        }

        attack_ElapsedTime -= Time.deltaTime;
        if (attack_ElapsedTime <= 0)
        {
            attack_ElapsedTime = timeToAttack;
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, enemy.transform.position, attackRange);
            if (raycastHit.point != (Vector2)enemy.transform.position)
            {
                OnAttack?.Invoke(this, new OnAttackEventArgs
                {
                    enemy = enemy.transform
                });

                if (useExternailDamageCode) return;

                iDamageableEnemy.Damage(damage);
            }
        }
    }

    #endregion


    #region Event

    private void SwordsMan_OnReachedDestination(object sender, System.EventArgs e)
    {
        state = States.Idle;
        attack_ElapsedTime = timeToAttack;
    }


    private void HealthSystem_OnHealthDepleted(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
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
        if (numberOfUpgrades >= maxNumberOfUpgrades) return;

        // Attack Range
        int cost = 20;
        if (teamManager.TryUseMinerials(cost))
        {
            int rangeIncrease = 1;
            attackRange += rangeIncrease;
            startAttackingRange += rangeIncrease;
            numberOfUpgrades++;
        }
    }

    public void OnSlot2ButtonClicked()
    {
        if (numberOfUpgrades >= maxNumberOfUpgrades) return;

        // HP
        int cost = 40;
        if (teamManager.TryUseMinerials(cost))
        {
            int healthIncrease = 1;
            healthSystem.AddMaxHealth(healthIncrease);
            healthSystem.Heal(healthIncrease);
            numberOfUpgrades++;
        }
    }

    public void OnSlot3ButtonClicked()
    {
        // Dmage
        if (numberOfUpgrades >= maxNumberOfUpgrades) return;

        int cost = 50;
        if (teamManager.TryUseMinerials(cost))
        {
            int damageIncrease = 1;
            damage += damageIncrease;
            numberOfUpgrades++;
        }
    }


    public void Damage(int amount)
    {
        healthSystem.Damage(amount);
    }

    #endregion


}
