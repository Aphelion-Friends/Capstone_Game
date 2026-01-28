using UnityEngine;
using PurrNet;

public class Spider : GenericEnemy
{
    [SerializeField] private NetworkAnimator _animator;

    public override void OnDeath()
    {
        _animator.SetTrigger("Die");

        base.OnDeath();
    }
}
