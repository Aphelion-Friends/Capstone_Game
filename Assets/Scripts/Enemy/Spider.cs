using UnityEngine;
using PurrNet;

public class Spider : GenericEnemy
{
    [SerializeField] private NetworkAnimator _animator;

    private MultiAudioSource audioSource, audioSource2;

    public override void OnDeath()
    {
        _animator.SetTrigger("Die");
        ObjectiveManager.Instance.currentState.objective.EnemyKilled("spider");

        base.OnDeath();
    }

    protected override void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        base.AttackTransitions(ref state);
        audioSource.Play();
    }

    protected override void PatrolTransitions(ref EnemyPatrolState.PatrolState state)
    {
        base.PatrolTransitions(ref state);
    }

    protected override void ChaseTransitions(ref EnemyChaseState.ChaseState state)
    {
        base.ChaseTransitions(ref state);
        //Work in progress need to add some kind of sound cooldown.
        // audioSource2.Play();
    }

    protected override void Awake()
    {
        base.Awake();
        audioSource = MultiAudioSource.FromResource(this.gameObject, "Spiderattack");
        audioSource2 = MultiAudioSource.FromResource(this.gameObject, "Spidercrawl");
    }
}
