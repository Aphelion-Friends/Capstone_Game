// Using this tutorial: https://www.youtube.com/watch?v=-Iwsz4gdgyQ

using UnityEngine;
using UnityEngine.AI;
using PurrNet;

public class enemyAIPatrol : NetworkIdentity
{
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

    NetworkAnimator animator;

    SyncVar<bool> dead = new(false, ownerAuth:true);


    protected override void OnSpawned(bool asServer)
    {
        base.OnSpawned(asServer);
        // if (asServer)
        //     GiveOwnership(PlayerID.Server);

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<NetworkAnimator>();

        networkManager.onTick += OnTick;
    }

    // Update is called once per frame
    private void OnTick(bool asServer)
    {
        if (!dead && asServer)
        {
            GameObject targetedPlayer = null;
            Collider[] playerInSightColliders = Physics.OverlapSphere(transform.position, sightRange, playerLayer);
            Collider[] playerInAttackRangeColliders = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

            playerInSight = playerInSightColliders.Length > 0;
            playerInAttackRange = playerInAttackRangeColliders.Length > 0;

            if (playerInAttackRange)
            {
                // Debug.Log("ATTAK");
                targetedPlayer = GetClosestPlayer(playerInAttackRangeColliders);
            }
            else if (playerInSight)
            {
                targetedPlayer = GetClosestPlayer(playerInSightColliders);
            }

            if (!playerInSight && !playerInAttackRange) Patrol();

            if (targetedPlayer is null)
                return;

            if (playerInSight && !playerInAttackRange) Chase(targetedPlayer);
            if (playerInSight && playerInAttackRange) Attack(targetedPlayer);
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

    void Attack(GameObject playerAttacked)
    {
        float timeSinceLastAttack = Time.time - timeAtLastAttack;
        animator.SetTrigger("Attack");
        NetworkIdentity playerAttackedOwner = playerAttacked.GetComponent<NetworkIdentity>();

        if (timeSinceLastAttack >= attackCooldown && playerAttackedOwner is not null)
        {
            // playerAttacked.GetComponent<PlayerHealth>().TakeDamage(10f);
            DealDamage(playerAttackedOwner.owner.Value, 10f);
            timeAtLastAttack = Time.time;
        }
    }

    [TargetRpc]
    void DealDamage(PlayerID target, float damage)
    {
        FindFirstObjectByType<PlayerHealth>().TakeDamage(damage);
    }

    void Chase(GameObject targetedPlayer)
    {
        agent.SetDestination(targetedPlayer.transform.position);
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

    [ServerRpc]
    public void Die()
    {
        // Debug.Log("SHOUD DIE");
        Debug.Log(dead.value);
        dead.value = true;
        OnDie(dead.value);
        agent.SetDestination(transform.position);
        animator.SetTrigger("Die");
    }

    // Makes the enemy appear dead for the clients
    // Should be called when the server sets the spider to be dead
    [ObserversRpc]
    private void OnDie(bool isDead)
    {
        // Debug.Log("DEAD!");
        if (isDead)
        {
            GetComponent<Collider>().enabled = false;
        }
    }
}
