using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(EnemyStunState))]
[RequireComponent(typeof(EnemyController))]
public class Psychotron : GenericEnemy
{
    [SerializeField] private Animator animator;
    private EnemyHealth enemyHealth;
    private EnemyStunState enemyStunState;
    private EnemyController enemyController;

    private MultiAudioSource audioSource, stunAudio;

    protected override void Awake()
    {
        base.Awake();

        enemyHealth = GetComponent<EnemyHealth>();
        enemyStunState = GetComponent<EnemyStunState>();
        enemyController = GetComponent<EnemyController>();
        enemyStunState.transitionFunc = StunTransitions;
        audioSource = MultiAudioSource.FromResource(this.gameObject, "Robotstep");
        stunAudio = MultiAudioSource.FromResource(this.gameObject, "Stun");
        // multiAudioSource = MultiAudioSource.
        
    }

    protected void StunTransitions(ref EnemyStunState.StunState state)
    {
        stateMachine.SetState(enemyPatrolState);
        animator.SetBool("Stunned", false);
        enemyController.active = true;
    }

    protected override void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        Debug.Log("Psychotron attak!!!");
        animator.SetTrigger("Attack");

        base.AttackTransitions(ref state);

        // multiAudioSource.PlayRandom();

    }

    public override void OnDeath()
    {
        // Psychotron does not die! He just gets stunned when he "dies"

        stateMachine.SetState(enemyStunState);
        enemyHealth.Reset();
        animator.SetBool("Stunned", true);
        enemyController.active = false;
        Debug.Log("Stun audio plays");
        stunAudio.PlayOnlyIfDone();
    }

    protected override void ChaseTransitions(ref EnemyChaseState.ChaseState state)
    {
        base.ChaseTransitions(ref state);
        audioSource.PlayOnlyIfDone();
    }
}
