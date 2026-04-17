using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;

    private PlayerShoot playerShoot;

    public void SetPlayer(PlayerShoot newPlayerShoot)
    {
        playerShoot = newPlayerShoot;
    }

    private void Update()
    {
        if (ammoText == null)
            return;

        if (playerShoot == null)
        {
            ammoText.text = "Ammo: 0 / 0";
            return;
        }

        ammoText.text = $"{playerShoot.CurrentAmmo} / {playerShoot.ReserveAmmo}";
    }
}