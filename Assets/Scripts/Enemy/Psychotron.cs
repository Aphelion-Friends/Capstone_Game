using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

public class Psychotron : GenericEnemy
{
    [SerializeField] private Animator animator;
    private EnemyHealth enemyHealth;

    protected override void Awake()
    {
        base.Awake();

        enemyHealth = GetComponent<EnemyHealth>();
    }

    protected override void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        Debug.Log("Psychotron attak!!!");
        animator.SetTrigger("Attack");

        base.AttackTransitions(ref state);
    }

    public override void OnDeath()
    {
        // Psychotron does not die! He just gets stunned when he "dies"

        enemyHealth.Reset();
    }
}
