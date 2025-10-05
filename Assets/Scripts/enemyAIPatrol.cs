// Using this tutorial: https://www.youtube.com/watch?v=-Iwsz4gdgyQ

using UnityEngine;
using UnityEngine.AI;

public class enemyAIPatrol : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float walkRange;

    float timeAtLastDestSet;
    [SerializeField] float giveUpTime = 5f;

    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    float timeAtLastAttack;
    [SerializeField] float attackCooldown = 1f;

    Animator animator;

    bool dead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            Collider[] playerInSightColliders = Physics.OverlapSphere(transform.position, sightRange, playerLayer);
            Collider[] playerInAttackRangeColliders = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

            playerInSight = playerInSightColliders.Length > 0;
            playerInAttackRange = playerInAttackRangeColliders.Length > 0;

            if (playerInAttackRange)
            {
                // Debug.Log("ATTAK");
                player = GetClosestPlayer(playerInAttackRangeColliders);
            }
            else if (playerInSight)
            {
                player = GetClosestPlayer(playerInSightColliders);
            }

            if (!playerInSight && !playerInAttackRange) Patrol();
            if (playerInSight && !playerInAttackRange) Chase();
            if (playerInSight && playerInAttackRange) Attack();
        }
    }

    GameObject GetClosestPlayer(Collider[] colliderArray)
    {
        GameObject currentBest = colliderArray[0].gameObject;
        for (int x = 1; x < colliderArray.Length; x++)
        {
            float currentBestDistance = Vector3.Distance(transform.position, currentBest.transform.position);
            float currentDistance = Vector3.Distance(transform.position, colliderArray[x].gameObject.transform.position);

            if (currentDistance < currentBestDistance)
            {
                currentBest = colliderArray[x].gameObject;
            }
        }
        return currentBest;
    }

    void Attack()
    {
        float timeSinceLastAttack = Time.time - timeAtLastAttack;
        animator.SetTrigger("Attack");
        // agent.SetDestination(transform.position);

        if (timeSinceLastAttack >= attackCooldown)
        {
            FindFirstObjectByType<PlayerHealth>().TakeDamage(10f);
            timeAtLastAttack = Time.time;
        }
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
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

    public void HearSound(Vector3 soundLocation)
    {
        if (!playerInSight && !playerInAttackRange)
        {
            destPoint = soundLocation;
            walkPointSet = true;
            timeAtLastDestSet = Time.time;
        }
    }

    public void Die()
    {
        dead = true;
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
    }
}
