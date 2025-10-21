using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI parts")]
    public Image healthBarFill;
    public TMP_Text healthText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hurtSound;
    private float lastHurtSoundTime;
    public float hurtSoundCooldown = 0.2f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if(hurtSound != null && audioSource != null && Time.time - lastHurtSoundTime > hurtSoundCooldown)
        {
            audioSource.PlayOneShot(hurtSound);
            lastHurtSoundTime = Time.time;
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


