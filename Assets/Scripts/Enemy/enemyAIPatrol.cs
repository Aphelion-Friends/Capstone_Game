// Adapted from this tutorial: https://www.youtube.com/watch?v=-Iwsz4gdgyQ

using UnityEngine;
using UnityEngine.AI;
using PurrNet.Prediction;
using Unity.VisualScripting;
using UnityEditor.Rendering;

public class EnemyAIPatrol : PredictedIdentity<EnemyAIPatrol.EnemyState>
{
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;

    [SerializeField] private float giveUpTime;

    [SerializeField] float walkRange;

    float timeAtLastDestSet;
    [SerializeField] float destPointCooldown = 5f;

    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    float timeAtLastAttack;
    [SerializeField] float attackCooldown = 1f;

    [SerializeField] float minDistance = 2f;

    public struct EnemyState : IPredictedData<EnemyState>
    {
        public Vector3 destPoint;
        public bool destPointSet;
        public float destSetCooldownTimer;

        public bool playerInSight;
        public bool playerInAttackRange;

        public float attackCooldownTimer;

        public float giveUpTimer;

        public void Dispose() {}
    }

    protected override void LateAwake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Simulate(ref EnemyState state, float delta)
    {
        state.destSetCooldownTimer -= delta;

        state.attackCooldownTimer -= delta;

        state.giveUpTimer -= delta;

        float distance = Vector3.Distance(state.destPoint, transform.position);

        if (distance <= minDistance)
        {
            state.destPointSet = false;
        }

        if (state.destSetCooldownTimer <= 0)
        {
            state.destPointSet = false;

            state.destSetCooldownTimer = destPointCooldown; 
        }

        if (state.attackCooldownTimer <= 0)
        {
            state.attackCooldownTimer = attackCooldown;
        }

        if (state.destPointSet == true && state.giveUpTimer <= 0)
        {
            state.giveUpTimer = giveUpTime;

            state.destPointSet = false;
        }

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

    //NEEDS TO BE REDONE COMPLETELY 
    void Attack(GameObject playerAttacked)
    {
        if (playerAttacked is not null)
        {    
            playerAttacked.GetComponent<PlayerHealth>().ChangeHealth(-10f);
        }
    }


    void Chase(GameObject targetedPlayer)
    {
        agent.SetDestination(targetedPlayer.transform.position);
    }

    void Patrol()
    {
        if (!currentState.destPointSet)
        {
            SearchForDest();
        }
        else if (currentState.destPointSet)
        {
            agent.SetDestination(currentState.destPoint);
        }

    }

    void SearchForDest()
    {
        float x = Random.Range(-walkRange, walkRange);
        float z = Random.Range(-walkRange, walkRange);

        currentState.destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(currentState.destPoint, Vector3.down, groundLayer))
        {
            currentState.destPointSet = true;
        }
    }

    // public void HearSound(Vector3 soundLocation)
    // {
    //     if (!playerInSight && !playerInAttackRange)
    //     {
    //         destPoint = soundLocation;
    //         walkPointSet = true;
    //         timeAtLastDestSet = Time.time;
    //     }
    // }

    // [ServerRpc]
    // public void Die()
    // {
    //     // Debug.Log("SHOUD DIE");
    //     if (!dead.value)
    //     {
    //         Debug.Log(dead.value);
    //         dead.value = true;
    //         OnDie(dead.value);
    //         agent.SetDestination(transform.position);
    //         animator.SetTrigger("Die");
    //         EnemySpawner.Instance.spawnEnemy(0);

    //     }
    // }

    // // Makes the enemy appear dead for the clients
    // // Should be called when the server sets the spider to be dead
    // [ObserversRpc]
    // private void OnDie(bool isDead)
    // {
    //     // Debug.Log("DEAD!");
    //     if (isDead)
    //     {
    //         //GetComponent<Collider>().enabled = false;


    //      
    //         ObjectiveManager.Instance.objective.EnemyKilled("spider");
    //         gameObject.layer = 7;
    //         gameObject.GetComponent<Item>().enabled = true;
    //         gameObject.GetComponent<NavMeshAgent>().enabled = false;
    //         gameObject.GetComponent<enemyAIPatrol>().enabled = false;

    //   
    //     }
    // }
}
