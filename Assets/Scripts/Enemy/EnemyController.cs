using UnityEngine;
using UnityEngine.AI;
using PurrNet.Prediction;

public class EnemyController : PredictedIdentity<EnemyController.ControllerState>
{
    private NavMeshAgent _agent;

    private float _radius = 1f;
    private float _height = 1f;

    public bool active { get => currentState.active; set { currentState.active = value; SetActive(); } }

    public float speed { get => currentState.speed; set => currentState.speed = Mathf.Abs(value); }

    protected override void LateAwake()
    {
        _agent = gameObject.AddComponent<NavMeshAgent>();
        _agent.autoRepath = false;
        _agent.speed = currentState.speed;
        _agent.height = _height;
    }

    private void SetActive()
    {
        _agent.enabled = active;
    }

    public struct ControllerState : IPredictedData<ControllerState>
    {
        public bool active;
        public Vector3 destPoint;
        public float speed;

        public void Dispose() {}
    }
}
