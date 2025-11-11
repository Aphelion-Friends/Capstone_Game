using UnityEngine;
using StarterAssets;

public class PlayerADS : MonoBehaviour
{
    public StarterAssetsInputs input;
    private bool isAiming = false;
    [SerializeField] private AdjustGunPosition adjustGunPosition;
    private float zOffsetBeforeADS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zOffsetBeforeADS = adjustGunPosition.relativeGunOffset.z;
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
        adjustGunPosition.relativeGunOffset.z = 0;
    }

    void StopADS()
    {
        adjustGunPosition.relativeGunOffset.z = zOffsetBeforeADS;
    }
}
