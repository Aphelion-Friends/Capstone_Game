using UnityEngine;
using StarterAssets;

public class PlayerADS : MonoBehaviour
{
    public StarterAssetsInputs input;
    private bool isAiming = false;
    public GameObject gun;
    private float gunRelativeXBeforeADS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunRelativeXBeforeADS = gun.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.aim && !isAiming)
        {
            isAiming = true;
            StartADS();
        }
        else if (!input.aim && isAiming)
        {
            isAiming = false;
            StopADS();
        }
    }

    void StartADS()
    {
        gun.transform.localPosition = new Vector3(0, gun.transform.localPosition.y, gun.transform.localPosition.z);
    }

    void StopADS()
    {
        gun.transform.localPosition = new Vector3(gunRelativeXBeforeADS, gun.transform.localPosition.y, gun.transform.localPosition.z);
    }
}
