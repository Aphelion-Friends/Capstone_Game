using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerHealth
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    private bool isDead = false;

    [Header("UI parts")]
    public Image healthBarFill;
    public TMP_Text healthText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    private float lastHurtSoundTime;
    public float hurtSoundCooldown = 0.2f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            playDeathSound();
            //Things that happens when the player dies will also go here, like a respawn or something (maybe animation calls? however animations work in Unity lol)
        }
        else
        {
            if (Time.time - lastHurtSoundTime > hurtSoundCooldown)
            {
                playHurtSound();
                lastHurtSoundTime = Time.time;
            }
        }
    }
    //[ObserverRPC]
    void playHurtSound()
    {
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }

    //[ObserversRpc]
    void playDeathSound()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    void UpdateHealthUI()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
    
    //Temporaraily is in place to test damage (*REMOVE AFTER MOBS DO DAMAGE*)
    void Update()
    {
        // Using the new Input System
        // if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     TakeDamage(10f);
        // }
    }

    
}


