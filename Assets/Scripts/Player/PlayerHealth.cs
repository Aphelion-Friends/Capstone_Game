using UnityEngine;
using PurrNet.Prediction;

public class PlayerHealth : PredictedIdentity<PlayerHealth.HealthState>
{
    [SerializeField] private float _maxHealth = 100f;
    public GameObject corpse;


    protected override HealthState GetInitialState()
    {
        return new HealthState
        {
            health = _maxHealth,
            isDead = false,
        };
    }

    public bool ChangeHealth(float change)
    {
        currentState.health += change;
        Debug.Log("Current HP: " + currentState.health);

        if (currentState.health <= 0 && !currentState.isDead)
        {
            currentState.isDead = true;
            Die();
        }

        return currentState.isDead;
    }

    private void Die()
    {
        //this.GetComponent<PlayerShoot>().enabled = false;
        //this.GetComponent<PlayerMovement>().enabled = false;
        //this.GetComponent<NetworkInventory>().enabled = false;

        //MeshRenderer[] visuals = this.GetComponentsInChildren<MeshRenderer>();

        gameObject.tag = "Dead";
        gameObject.SetActive(false);

        GameObject newCam = GameObject.FindWithTag("MainCamera");
        newCam.GetComponent<Camera>().enabled = true;
        //spawnCorpse();

        Debug.Log("The player died!");
        currentState.isDead = true;
    }

    private void spawnCorpse()
    {
        GameObject body = Instantiate(corpse);
        corpse.GetComponent<Camera>().enabled = false;
    }

    public struct HealthState : IPredictedData<HealthState>
    {
        public float health;
        public bool isDead;
        public override string ToString()
        {
            return $"Health: {health}";
        }

        public void Dispose() {}
    }

}


