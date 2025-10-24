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

        // Only enable HUD for the owner of this player
        hud.SetActive(isOwner);
    }
}
