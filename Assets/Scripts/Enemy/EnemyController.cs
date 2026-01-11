using UnityEngine;
using UnityEngine.AI;
using PurrNet.Prediction;

[RequireComponent(typeof(PredictedRigidbody))]
public class EnemyController : PredictedIdentity<EnemyController.ControllerState>
{
    private NavMeshAgent _agent;

    [SerializeField] private float _radius = 1f;
    [SerializeField] private float _height = 1f;

    // Choose a value that is not too small because the enemy movement will get too twitchy but also not too large
    [SerializeField] private float _warpDistance = 10f;

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


    protected override void Simulate(ref ControllerState state, float delta)
    {
        _agent.enabled = active;

        if (!active)
            return;

        _agent.speed = state.speed;
        _agent.destination = state.destPoint;

        if (Vector3.Magnitude(_agent.nextPosition - transform.position) > _warpDistance)
        {
            Debug.Log("Warping!");
            _agent.Warp(transform.position);
        }
    }

    protected override ControllerState GetInitialState()
    {
        return new ControllerState{
            active = true,
            destPoint = new Vector3(),
            speed = 5f
        };
    }

    public struct ControllerState : IPredictedData<ControllerState>
    {
        public bool active;
        public Vector3 destPoint;
        public float speed;

        public void Dispose() {}
    }
}
