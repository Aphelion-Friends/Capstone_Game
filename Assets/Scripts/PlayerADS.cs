using UnityEngine;
using StarterAssets;

public class PlayerADS : MonoBehaviour
{
    public StarterAssetsInputs input;
    private bool isAiming = false;
    public GameObject gun;
    private float gunRelativeXBeforeADS;

    private float timeForADS = 0.1f;
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
        LeanTween.moveLocalX(gun, 0, timeForADS).setEase(LeanTweenType.easeInOutCubic);

    }

    void StopADS()
    {
        LeanTween.moveLocalX(gun, gunRelativeXBeforeADS, timeForADS).setEase(LeanTweenType.easeInOutCubic);
    }
}
