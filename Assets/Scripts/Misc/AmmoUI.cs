using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;

    private PlayerShoot playerShoot;

    public void SetPlayer(PlayerShoot newPlayerShoot)
    {
        if (newPlayerShoot == null)
            return;

        if (!newPlayerShoot.isOwner)
            return;

        playerShoot = newPlayerShoot;
    }

    private void Update()
    {
        if (ammoText == null)
            return;

        if (playerShoot == null)
        {
            ammoText.text = "0 / 0";
            return;
        }

        if (!playerShoot.isActiveAndEnabled)
        {
            ammoText.text = "0 / 0";
            playerShoot = null;
            return;
        }

        ammoText.text = $"{playerShoot.CurrentAmmo} / {playerShoot.ReserveAmmo}";
    }
}