using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    [SerializeField] private GameObject visual;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private AttackingUnit attackingUnit;


    private void Start()
    {
        attackingUnit.OnAttack += AttackingUnit_OnAttack;
    }

    private async void AttackingUnit_OnAttack(object sender, AttackingUnit.OnAttackEventArgs e)
    {
        visual.SetActive(true);
        await System.Threading.Tasks.Task.Delay(250);
        transform.right = attackingUnit.GetDirectionToEnemy();
        Arrow arrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90));
        arrow.Init(e.enemy, attackingUnit.GetDamageAmount());
        await System.Threading.Tasks.Task.Delay(250);
        visual.SetActive(false);
    }

}
