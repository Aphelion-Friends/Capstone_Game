using UnityEngine;
using PurrNet;

[RequireComponent(typeof(EnemyAttackCooldown))]
public class Spider : GenericEnemy
{
    [SerializeField] private NetworkAnimator _animator;

    private MultiAudioSource audioSource, audioSource2;

    private EnemyAttackCooldown enemyAttackCooldown;

    public override void OnDeath()
    {
        _animator.SetTrigger("Die");
        ObjectiveManager.Instance.currentState.objective.EnemyKilled("spider");

        base.OnDeath();
    }

    protected override void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        base.AttackTransitions(ref state);
        enemyAttackCooldown.ResetTimer();
        audioSource.PlayOnlyIfDone();
        stateMachine.SetState(enemyChaseState);
    }

    protected override void PatrolTransitions(ref EnemyPatrolState.PatrolState state)
    {
        base.PatrolTransitions(ref state);
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
        audioSource2.PlayOnlyIfDone();
    }

    protected override void Awake()
    {
        base.Awake();
        audioSource = MultiAudioSource.FromResource(this.gameObject, "Spiderattack");
        audioSource2 = MultiAudioSource.FromResource(this.gameObject, "Spidercrawl");
        enemyAttackCooldown = GetComponent<EnemyAttackCooldown>();
    }
}
