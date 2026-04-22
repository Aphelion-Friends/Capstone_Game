using UnityEngine;
using PurrNet;
using UnityEngine.Audio;

public class Spider : GenericEnemy
{
    [SerializeField] private NetworkAnimator _animator;

    private MultiAudioSource audioSource, audioSource2, spiderDeathAudio;

    public override void OnDeath()
    {
        _animator.SetTrigger("Die");
        ObjectiveManager.Instance.currentState.objective.EnemyKilled("spider");
        

        base.OnDeath();
        Debug.Log("Spider death audio played");
        spiderDeathAudio.PlayOnlyIfDone();
        spiderDeathAudio.SetVolume(0.5f);
    }

    protected override void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        base.AttackTransitions(ref state);
        audioSource.PlayOnlyIfDone();
    }

    protected override void PatrolTransitions(ref EnemyPatrolState.PatrolState state)
    {
        base.PatrolTransitions(ref state);
    }

    protected override void ChaseTransitions(ref EnemyChaseState.ChaseState state)
    {
        base.ChaseTransitions(ref state);
        audioSource2.PlayOnlyIfDone();
    }

    protected override void Awake()
    {
        base.Awake();
        audioSource = MultiAudioSource.FromResource(this.gameObject, "Spiderattack");
        audioSource2 = MultiAudioSource.FromResource(this.gameObject, "Spidercrawl");
        spiderDeathAudio = MultiAudioSource.FromResource(this.gameObject, "spiderdeath");
    }
}
