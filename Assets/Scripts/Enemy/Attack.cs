using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _attackCooldown;

    void DamagePlayer(GameObject playerAttacked)
    {
        if (playerAttacked is not null)
        {    
            playerAttacked.GetComponent<PlayerHealth>().ChangeHealth(-10f);
        }
    }
}
