using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{

    private const string SWING_SWORD_ANIMATION_NAME = "SwingSword";


    [SerializeField] private Animator animator;
    [SerializeField] private AttackingUnit attackingUnit;


    private void Start()
    {
        attackingUnit.OnAttack += AttackingUnit_OnAttack;
    }

    private void AttackingUnit_OnAttack(object sender, AttackingUnit.OnAttackEventArgs e)
    {
        animator.SetTrigger(SWING_SWORD_ANIMATION_NAME);
        transform.up = attackingUnit.GetDirectionToEnemy();
    }
}
