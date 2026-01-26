using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

public class Psychotron : GenericEnemy
{
    [SerializeField] private Animator animator;

    protected override void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        base.AttackTransitions(ref state);

        animator.SetTrigger("Attack");
    }
}
