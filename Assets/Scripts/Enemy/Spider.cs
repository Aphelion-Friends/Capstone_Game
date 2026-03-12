using UnityEngine;
using PurrNet;

public class Spider : GenericEnemy
{
    [SerializeField] private NetworkAnimator _animator;

    private MultiAudioSource audioSource;

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

    protected override void Awake()
    {
        base.Awake();
        audioSource = MultiAudioSource.FromResource(this.gameObject, "Spiderattack");
    }
}
