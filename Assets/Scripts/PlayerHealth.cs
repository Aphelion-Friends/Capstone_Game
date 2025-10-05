using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Image healthBarFill;
    public TMP_Text healthText;

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


