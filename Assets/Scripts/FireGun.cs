//Most of this script comes from the Brackey Unity Gun tutorial 
// You can check it out here --> https://www.youtube.com/watch?v=THnivyG0Mvo&list=PLPV2KyIb3jR7dFbE2UQYu7QWMdUgDnlnk 

using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GunFireScript : MonoBehaviour
{

    public UpdateHUD HUDScript;

    public GunSound gunSoundScript;

    public StarterAssetsInputs input;
    
    public float damage = 10f; //Change these values if you want to make the damage or range bigger or smaller
    public float range = 100f; //Ideally I'd like to have other variables like gun recoil 
    public float gunRecoil = 20f;
    public Camera fpsCam;

    int ammoCount = 0; // Current ammo in gun
    public int ammoCapacity = 0; // Max ammo for gun

    public GunRecoil gunRecoilScript;
    
    void Start()
    {
        Reload();
        HUDScript.SetAmmo(ammoCount);
    }

    void Update()
    {
        if (input.click)
        {
            Shoot();
            input.click = false;
        }

        if (input.reload)
        {
            Reload();
            input.reload = false;
        }
    }

    void Reload()
    {
        ammoCount = ammoCapacity;
        HUDScript.SetAmmo(ammoCount);
    }

    void UseAmmo()
    {
        ammoCount--;
        HUDScript.SetAmmo(ammoCount);
    }

    void Shoot()
    {
        if (ammoCount > 0)
        {
            gunSoundScript.PlayGunshotSound();
            gunRecoilScript.Recoil(gunRecoil);
            RaycastHit hitInfo;

            if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
            {
                Debug.Log("Bang!");                         //Everytime you fire this will be send into the debug log
                //Debug.Log(hitInfo.transform.name);        //Optionally you can uncomment this line of code and it'll tell you what you fired at
            }    
            UseAmmo();
        }
        
    }
}

