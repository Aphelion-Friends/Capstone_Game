using UnityEngine;
using StarterAssets;

public class PlayerADS : MonoBehaviour
{
    public StarterAssetsInputs input;
    private bool isAiming = false;
    public GameObject gun;
    private float gunRelativeYBeforeADS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunRelativeYBeforeADS = gun.transform.localPosition.y;
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
        gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, 0, gun.transform.localPosition.z);
    }

    void StopADS()
    {
        gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, gunRelativeYBeforeADS, gun.transform.localPosition.z);
    }
}
