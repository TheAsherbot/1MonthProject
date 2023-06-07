using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{


    private const string IDLE_NAME = "Idle";
    private const string MOVEUP_NAME = "MoveUp";
    private const string MOVELEFT_NAME = "MoveLeft";
    private const string MOVEDOWN_NAME = "MoveDown";
    private const string MOVERIGHT_NAME = "MoveRight";



    [Header("Generial Animatio")]
    private Animator animator;
    
    [Header("References")]
    [SerializeField] private UnitMovement unitMovement;


    private void Start()
    {
        animator = GetComponent<Animator>();

        unitMovement.OnDirectionChanged += UnitMovement_OnDirectionChanged;
    }

    private void UnitMovement_OnDirectionChanged(object sender, UnitMovement.OnDirectionChangedEventArgs e)
    {
        if (Mathf.Abs(Vector2.Distance(e.direction, Vector2.zero)) < 0.25f)
        {
            animator.SetTrigger(IDLE_NAME);
        }
        else if (Mathf.Abs(Vector2.Distance(e.direction, Vector2.up)) < 0.25f)
        {
            animator.SetTrigger(MOVEUP_NAME);
        }
        else if (Mathf.Abs(Vector2.Distance(e.direction, Vector2.left)) < 0.25f)
        {
            animator.SetTrigger(MOVELEFT_NAME);
        }
        else if (Mathf.Abs(Vector2.Distance(e.direction, Vector2.down)) < 0.25f)
        {
            animator.SetTrigger(MOVEDOWN_NAME);
        }
        else if (Mathf.Abs(Vector2.Distance(e.direction, Vector2.right)) < 0.25f)
        {
            animator.SetTrigger(MOVERIGHT_NAME);
        }
    }

}
