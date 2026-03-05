using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

public class EnemyAttackState : PredictedStateNode<EnemyAttackState.AttackState>
{
    public delegate void TransitionFunc(ref EnemyAttackState.AttackState state);
    public TransitionFunc transitionFunc;

    [SerializeField] private PredictedRigidbody attackRigidbody;

    protected override void LateAwake()
    {
        attackRigidbody.onCollisionEnter += OnCollision;
    }

    private void OnCollision(GameObject other, PhysicsCollision physicsCollision)
    {
        PredictedObjectID playerID;

        if (!predictionManager.hierarchy.TryGetId(other, out playerID))
            return;

        currentState.targetedPlayer = playerID;
    }

    public struct AttackState : IPredictedData<AttackState>
    {
        public PredictedObjectID? targetedPlayer;

        public void Dispose() {}
    }

    public override void Enter()
    {
        Debug.Log("Entered attack state!");
        //currentState.targetedPlayer = null;
    }

    protected override void StateSimulate(ref AttackState state, float delta)
    {
        transitionFunc(ref state);

        if (!state.targetedPlayer.HasValue)
            return;

        GameObject player;

        if (predictionManager.hierarchy.TryGetGameObject(currentState.targetedPlayer, out player))
        {
            Debug.Log("We got " + player);
            player.GetComponent<PlayerHealth>().ChangeHealth(-25f);
        }

        Debug.Log("Player" + state.targetedPlayer.Value + "attacked!");
    }
}
