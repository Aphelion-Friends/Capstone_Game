using PurrNet.Prediction;
using UnityEngine;

public class CriticalHealthOverlay : StatelessPredictedIdentity
{
    [Header("References")]
    private PlayerHealth playerHealth;
    [SerializeField] private GameObject halfHealthOverlay;
    [SerializeField] private GameObject quarterHealthOverlay;

    [Header("Thresholds")]
    [SerializeField] private float halfHealthThreshold = 0.5f;
    [SerializeField] private float quarterHealthThreshold = 0.25f;

    protected override void LateAwake()
    {
        playerHealth = GetComponent<PlayerHealth>();

        SetOverlayState(false, false);

        if (!isOwner)
        {
            enabled = false;
        }
    }

    protected override void Simulate(float delta)
    {
        if (!isOwner)
        {
            SetOverlayState(false, false);
            return;
        }

        if (playerHealth == null || playerHealth.MaxHealth <= 0f)
        {
            SetOverlayState(false, false);
            return;
        }

        float hpPercent = playerHealth.currentState.health / playerHealth.MaxHealth;

        bool showQuarter = hpPercent <= quarterHealthThreshold;
        bool showHalf = hpPercent <= halfHealthThreshold && hpPercent > quarterHealthThreshold;

        SetOverlayState(showHalf, showQuarter);
    }

    private void SetOverlayState(bool showHalf, bool showQuarter)
    {
        if (halfHealthOverlay != null)
        {
            halfHealthOverlay.SetActive(showHalf);
        }

        if (quarterHealthOverlay != null)
        {
            quarterHealthOverlay.SetActive(showQuarter);
        }
    }
}