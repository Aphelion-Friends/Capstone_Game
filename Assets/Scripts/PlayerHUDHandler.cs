using PurrNet;
using UnityEngine;

public class PlayerHUDHandler : NetworkBehaviour
{
    [SerializeField] private GameObject hud;

    private void Start()
    {
        if (hud == null)
        {
            Debug.LogError("HUD not assigned in PlayerHUDHandler!");
            return;
        }
        hud.SetActive(isOwner);
    }
}