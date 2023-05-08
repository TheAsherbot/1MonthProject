using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;
using UnityEngine.Rendering;

public class SwordsMan : _BaseUnit, ISelectable
{


    public enum States
    {
        Idle,
        Moving,
        MovingToAttack,
        Attacking,
    }

    #region Variables

    public bool IsSelected
    {
        get;
        set;
    }
    [field: SerializeField]
    public List<HotBarSlotSO> HotBarSlotSOList
    {
        get;
        set;
    }

    private States state;


    [Header("Attacking")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float startAttackRange = 0.5f;
    [SerializeField] private float timeToAttack;
    [SerializeField] private int damage;
    private float attack_ElapsedTime;

    private IDamageable iDamageableEnemy;
    private IsOnAITeam IsOnAITeamEnemy;

    #endregion


    #region Unity Functions

    private void Start()
    {
        attack_ElapsedTime = timeToAttack;
        OnReachedDestination += SwordsMan_OnReachedDestination;
    }

    private void Update()
    {
        if (IsSelected)
        {
            TestInput();
        }
        TestState();
    }

    #endregion


    #region Input

    private void TestInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MoveInputPressed();
        }
    }

    private void MoveInputPressed()
    {
        state = States.Moving;
        if (Mouse2D.TryGetObjectAtMousePosition(out GameObject hit))
        {
            if (hit.TryGetComponent(out IsOnAITeam enemy))
            {
                enemy.TryGetComponent(out iDamageableEnemy);
                IsOnAITeamEnemy = enemy;
                state = States.MovingToAttack;
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
        if (Vector2.Distance(transform.position, IsOnAITeamEnemy.transform.position) <= startAttackRange)
        {
            Trigger_OnStopMoveing();
            state = States.Attacking;
        }
    }

    private void AttackingState()
    {
        attack_ElapsedTime -= Time.deltaTime;
        if (attack_ElapsedTime <= 0)
        {
            attack_ElapsedTime = timeToAttack;
            iDamageableEnemy.Damage(damage);
        }
    }

    #endregion


    #region Event

    private void SwordsMan_OnReachedDestination(object sender, System.EventArgs e)
    {
        state = States.Idle;
        attack_ElapsedTime = timeToAttack;
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
        // Attack
    }

    public void OnSlot2ButtonClicked()
    {
        // Potroll
    }

    public void OnSlot3ButtonClicked()
    {
        // Only Attack
    }

    public void OnSlot4ButtonClicked()
    {
        // Upgrade 1
    }

    public void OnSlot5ButtonClicked()
    {
        // Upgrade 2
    }

    #endregion

    
}
