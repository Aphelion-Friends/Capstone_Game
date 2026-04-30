using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(EnemyStunState))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyAttackCooldown))]
public class Psychotron : GenericEnemy
{
    [SerializeField] private Animator animator;
    private EnemyHealth enemyHealth;
    private EnemyStunState enemyStunState;
    private EnemyController enemyController;

    private MultiAudioSource audioSource, attackSource;

    private EnemyAttackCooldown enemyAttackCooldown;

    protected override void Awake()
    {
        base.Awake();

        enemyHealth = GetComponent<EnemyHealth>();
        enemyStunState = GetComponent<EnemyStunState>();
        enemyController = GetComponent<EnemyController>();
        enemyStunState.transitionFunc = StunTransitions;
        audioSource = MultiAudioSource.FromResource(this.gameObject, "Robotstep");
        enemyAttackCooldown = GetComponent<EnemyAttackCooldown>();
        attackSource = MultiAudioSource.FromResource(this.gameObject, "Robotbigstep5");
        
    }

    protected void StunTransitions(ref EnemyStunState.StunState state)
    {
        stateMachine.SetState(enemyPatrolState);
        animator.SetBool("Stunned", false);
        enemyController.active = true;
    }

    protected override void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        animator.SetTrigger("Attack");

        base.AttackTransitions(ref state);

        enemyAttackCooldown.ResetTimer();
        stateMachine.SetState(enemyChaseState);
        attackSource.PlayOnlyIfDone();
        attackSource.SetVolume(0.4f);

    }

    public override void OnDeath()
    {
        // Psychotron does not die! He just gets stunned when he "dies"

        stateMachine.SetState(enemyStunState);
        enemyHealth.Reset();
        animator.SetBool("Stunned", true);
        enemyController.active = false;
    }

    protected override void ChaseTransitions(ref EnemyChaseState.ChaseState state)
    {
        GameObject targetedPlayer = predictionManager.hierarchy.GetGameObject(state.targetedPlayer);

        if (targetedPlayer == null)
        {
            state.targetedPlayer = null;
            stateMachine.SetState(enemyPatrolState);
        }

        if (Vector3.Distance(transform.position, targetedPlayer.transform.position) >= chaseRange)
        {
            stateMachine.SetState(enemyPatrolState);
        }
        else if (Vector3.Distance(transform.position, targetedPlayer.transform.position) <= attackRange &&
                enemyAttackCooldown.currentState.timer <= 0)
        {
            stateMachine.SetState(enemyAttackState);
        }
        audioSource.PlayOnlyIfDone();
    }
}
