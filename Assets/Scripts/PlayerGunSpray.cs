// Based on this tutorial: https://www.youtube.com/watch?v=geieixA4Mqc
using UnityEngine;

public class PlayerGunSpray : MonoBehaviour
{
    public GameObject gun;
    public GameObject mainCameraRoot;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    GunFireScript fireGun;
    PlayerADS playerADS;

    bool isAiming = false;

    void Update()
    {
        fireGun = gun.GetComponent<GunFireScript>();

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, fireGun.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, fireGun.snappiness * Time.fixedDeltaTime);
        
        mainCameraRoot.transform.localRotation = Quaternion.Euler(currentRotation);

        playerADS = GetComponent<PlayerADS>();
    }

    public void RecoilFire()
    {
        PlayerADS playerADS = GetComponent<PlayerADS>();
        if (playerADS.isAiming)
            targetRotation += new Vector3(fireGun.sprayXAim, Random.Range(-fireGun.sprayYAim, fireGun.sprayYAim), Random.Range(-fireGun.sprayZAim, fireGun.sprayZAim));
        else if (!playerADS.isAiming)
            targetRotation += new Vector3(fireGun.sprayX, Random.Range(-fireGun.sprayY, fireGun.sprayY), Random.Range(-fireGun.sprayZ, fireGun.sprayZ));
    }
}
