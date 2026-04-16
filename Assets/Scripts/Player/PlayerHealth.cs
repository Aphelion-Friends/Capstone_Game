using UnityEngine;
using PurrNet.Prediction;

public class PlayerHealth : PredictedIdentity<PlayerHealth.HealthState>
{
    [SerializeField] private float _maxHealth = 100f;
    public GameObject corpse;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => currentState.health;
    public float HealthPercent => _maxHealth > 0f ? currentState.health / _maxHealth : 0f;
    public bool IsDead => currentState.isDead;

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
        currentState.health = Mathf.Clamp(currentState.health, 0f, _maxHealth);

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

        if (gameObject != null)
        {
            gameObject.tag = "Dead";

            spawnCorpse();

            gameObject.SetActive(false);

            GameObject newCam = GameObject.FindWithTag("MainCamera"); // Search for another player's camera to spectate

            if (newCam != null)
            {
                newCam.GetComponent<Camera>().enabled = true;
            }

            Debug.Log("The player died!");
            currentState.isDead = true;
        }
    }

    private void spawnCorpse()
    {
        GameObject body = Instantiate(corpse);
        body.transform.position = this.transform.position;
    }

    public struct HealthState : IPredictedData<HealthState>
    {
        public float health;
        public bool isDead;

        public override string ToString()
        {
            return $"Health: {health}";
        }

        public void Dispose() { }
    }
}