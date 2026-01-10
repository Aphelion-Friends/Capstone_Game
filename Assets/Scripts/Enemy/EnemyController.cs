using UnityEngine;
using UnityEngine.AI;
using PurrNet.Prediction;

[RequireComponent(typeof(PredictedRigidbody))]
public class EnemyController : PredictedIdentity<EnemyController.ControllerState>
{
    private NavMeshAgent _agent;

    private float _radius = 1f;
    private float _height = 1f;

    // Choose a value that is not too small because the enemy movement will get too twitchy but also not too large
    private float _warpDistance = 10f;

    public bool active { get => currentState.active; set { currentState.active = value; SetActive(); } }

    public float speed { get => currentState.speed; set => currentState.speed = Mathf.Abs(value); }

    public Vector3 destination { get => currentState.destPoint; set { currentState.destPoint = value; } }

    void Awake()
    {
        _agent = gameObject.AddComponent<NavMeshAgent>();
        _agent.autoRepath = false;
        _agent.height = _height;
        _agent.enabled = true;
    }

    private void SetActive()
    {
        _agent.enabled = active;
    }


    protected override void Simulate()
    {

    }
    // protected override void SetUnityState(ControllerState state)
    // {
    //     Debug.Log("Setting state!");

    //     _agent.speed = state.speed;

    //     _agent.destination = currentState.destPoint;
    //     if (active && Vector3.Magnitude(_agent.nextPosition - gameObject.transform.position) > _warpDistance)
    //     {
    //         Debug.Log("Warping!");
    //         _agent.Warp(transform.position);
    //     }
    // }

    protected override ControllerState GetInitialState()
    {
        return new ControllerState{
            active = true,
            destPoint = new Vector3(),
            speed = 1
        };
    }

    public struct ControllerState : IPredictedData<ControllerState>
    {
        public bool active;
        public Vector3 destPoint;
        public float speed;
        public Vector3 position;
        public Quaternion rotation;

        public void Dispose() {}
    }
}
