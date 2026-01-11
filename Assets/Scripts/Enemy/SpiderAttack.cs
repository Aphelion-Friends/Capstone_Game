using UnityEngine;
using PurrNet.Prediction;

public class SpiderAttack : PredictedIdentity<SpiderAttack.AttackState>, IEnemyAttack
{
    public void Attack()
    {
        
    }

    public struct AttackState : IPredictedData<AttackState>
    {
        public void Dispose() {}
    }
}
