// Using this tutorial: https://www.youtube.com/watch?v=-Iwsz4gdgyQ

using UnityEngine;
using UnityEngine.AI;

public class enemyAIPatrol : MonoBehaviour
{
    [SerializeField] GameObject player;
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float walkRange;

    float timeAtLastDestSet;
    [SerializeField] float giveUpTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (!walkPointSet)
        {
            SearchForDest();
        }
        else if (walkPointSet)
        {
            agent.SetDestination(destPoint);
        }

        // It gives up eventually if it can't reach its destination.
        if (Vector3.Distance(transform.position, destPoint) < 10 || (Time.time - timeAtLastDestSet) >= giveUpTime)
        {
            walkPointSet = false;
        }
    }

    void SearchForDest()
    {
        float x = Random.Range(-walkRange, walkRange);
        float z = Random.Range(-walkRange, walkRange);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkPointSet = true;
            timeAtLastDestSet = Time.time;
        }
    }
}
