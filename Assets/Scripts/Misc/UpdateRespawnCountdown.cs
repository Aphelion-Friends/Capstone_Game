using UnityEngine;
using TMPro;

public class UpdateRespawnCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI respawnCounter;

    public void SetCounterValue(int newValue)
    {
        respawnCounter.text = newValue.ToString();
    }
}
