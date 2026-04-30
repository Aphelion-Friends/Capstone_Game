using UnityEngine;
using PurrNet;
using UnityEngine.Audio;

[RequireComponent(typeof(EnemyAttackCooldown))]
public class Spider : GenericEnemy
{
    [SerializeField] private NetworkAnimator _animator;

    private MultiAudioSource audioSource, audioSource2, spiderDeathAudio;

    private EnemyAttackCooldown enemyAttackCooldown;

    public override void OnDeath()
    {
        _animator.SetTrigger("Die");
        ObjectiveManager.Instance.currentState.objective.EnemyKilled("spider");
        

        base.OnDeath();
        Debug.Log("Spider death audio played");
        spiderDeathAudio.PlayOnlyIfDone();
        spiderDeathAudio.SetVolume(0.8f);
        spiderDeathAudio.SetPitch(1.5f);
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
        if (flashlightDetector != null && flashlightDetector.isLit && flashlightSense != null)
        {
            if (flashlightSense.reaction == FlashLightReaction.Flee)
            {
                enemyFleeState.fleeFrom = flashlightDetector.lightSource;
                stateMachine.SetState(enemyFleeState);
                return;
            }
            else if (flashlightSense.reaction == FlashLightReaction.Attracted)
            {
                enemyAttractedState.attractedTo = flashlightDetector.lightSource;
                stateMachine.SetState(enemyAttractedState);
                return;
            }
        }


        base.PatrolTransitions(ref state);
    }

    protected override void ChaseTransitions(ref EnemyChaseState.ChaseState state)
{
    GameObject targetedPlayer = predictionManager.hierarchy.GetGameObject(state.targetedPlayer);

    if (targetedPlayer == null)
    {
        state.targetedPlayer = null;
        stateMachine.SetState(enemyPatrolState);
        return;
    }

    float distance = Vector3.Distance(transform.position, targetedPlayer.transform.position);

    // Attack check first - don't interrupt if in attack range
    if (distance <= attackRange && enemyAttackCooldown.currentState.timer <= 0)
    {
        stateMachine.SetState(enemyAttackState);
        return;
    }

    // Flashlight check second - only if not in attack range
    if (flashlightDetector != null && flashlightDetector.isLit && flashlightSense != null)
    {
        if (flashlightSense.reaction == FlashLightReaction.Flee)
        {
            enemyFleeState.fleeFrom = flashlightDetector.lightSource;
            stateMachine.SetState(enemyFleeState);
            return;
        }
        else if (flashlightSense.reaction == FlashLightReaction.Attracted)
        {
            enemyAttractedState.attractedTo = flashlightDetector.lightSource;
            stateMachine.SetState(enemyAttractedState);
            return;
        }
    }

    // Normal chase/patrol transitions last
    if (distance >= chaseRange)
        stateMachine.SetState(enemyPatrolState);

    audioSource2.PlayOnlyIfDone();
}

    protected override void Awake()
    {
        base.Awake();
        audioSource = MultiAudioSource.FromResource(this.gameObject, "Spiderattack");
        audioSource2 = MultiAudioSource.FromResource(this.gameObject, "Spidercrawl");
        spiderDeathAudio = MultiAudioSource.FromResource(this.gameObject, "spiderdeath");
        enemyAttackCooldown = GetComponent<EnemyAttackCooldown>();
    }
}
