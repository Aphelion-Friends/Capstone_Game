using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

public class Psychotron : GenericEnemy
{
    [SerializeField] private Animator animator;

    protected override void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        Debug.Log("Psychotron attak!!!");
        animator.SetTrigger("Attack");

        base.AttackTransitions(ref state);
    }

    protected override void OnDeath()
    {
        // Psychotron does not die!
    }
}
