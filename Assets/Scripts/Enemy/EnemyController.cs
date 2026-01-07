using UnityEngine;
using UnityEngine.AI;
using PurrNet.Prediction;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private float _speed = 3f;
    private float _radius = 1f;
    private float _height = 1f;

    public float speed { get => _speed; set => _speed = Mathf.Abs(value); }

    void Awake()
    {
        _agent = gameObject.AddComponent<NavMeshAgent>();
        _agent.autoRepath = false;
        _agent.speed = _speed;
        _agent.height = _height;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
