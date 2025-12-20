// //Most of this script comes from the Brackey Unity Gun tutorial 
// // You can check it out here --> https://www.youtube.com/watch?v=THnivyG0Mvo&list=PLPV2KyIb3jR7dFbE2UQYu7QWMdUgDnlnk 

// using StarterAssets;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEngine.Windows;
// using UnityEngine.VFX;
// using PurrNet;

using UnityEngine;

public class GunFireScript : MonoBehaviour
{

//     private UpdateHUD HUDScript;

//     public GunSound gunSoundScript;
//     public VisualEffect muzzleFlash;

//     public StarterAssetsInputs input;
    
//     public float damage = 10f; //Change these values if you want to make the damage or range bigger or smaller
//     public float range = 100f; //Ideally I'd like to have other variables like gun recoil 
//     public float roundsPerMinute = 100f;
//     public float gunRecoil = 20f;

//     public float sprayX;
//     public float sprayY;
//     public float sprayZ;

//     public float sprayXAim;
//     public float sprayYAim;
//     public float sprayZAim;

//     public float snappiness;
//     public float returnSpeed;

//     public bool fullyAutomatic = false;
//     public Camera fpsCam;

//     int ammoCount = 0; // Current ammo in gun
//     public int ammoCapacity = 0; // Max ammo for gun

//     public GunRecoil gunRecoilScript;

//     [SerializeField] LayerMask enemyLayer;
//     [SerializeField] float enemyAlertRadius;

//     [SerializeField] PlayerGunSpray playerGunSpray;

//     float timeAtLastShot = 0;
    
//     void Start()
//     {
//         HUDScript = FindFirstObjectByType<UpdateHUD>();
//         Reload();
//         HUDScript.SetAmmo(ammoCount);
//     }

//     void Update()
//     {
//         if (InventoryUI.inventoryOpen)
//         {
//             return;
//         }
//         if (input.click)
//         {
//             Shoot();
//             if (!fullyAutomatic)
//             {
//                 input.click = false;
//             }
//         }

//         if (input.reload)
//         {
//             Reload();
//             input.reload = false;
//         }

//         if (input.selectFire)
//         {
//             SelectFire();
//             input.selectFire = false;
//         }
//     }

//     void Reload()
//     {
//         ammoCount = ammoCapacity;
//         HUDScript.SetAmmo(ammoCount);
//     }

//     void UseAmmo()
//     {
//         ammoCount--;
//         HUDScript.SetAmmo(ammoCount);
//     }

//     void Shoot()
//     {
//         float shootDelay = (1 / roundsPerMinute) * 60;

//         if (ammoCount > 0 && (Time.time - timeAtLastShot) >= shootDelay)
//         {
//             timeAtLastShot = Time.time;

//             gunRecoilScript.Recoil(gunRecoil);
//             // gunSoundScript.PlayGunshotSound();
//             RaycastHit hitInfo;

//             if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
//             {
//                 Debug.Log("Bang!");                         //Everytime you fire this will be send into the debug log
//                 if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemies"))
//                 {
//                     hitInfo.transform.gameObject.GetComponent<enemyAIPatrol>().Die();
//                 }
//                 //Debug.Log(hitInfo.transform.name);        //Optionally you can uncomment this line of code and it'll tell you what you fired at
//             }
//             UseAmmo();
//             AlertEnemies(gameObject);
//             PlayGunEffects(gameObject);
//             playerGunSpray.RecoilFire();
//         }

//     }

//     [ObserversRpc]
//     void PlayGunEffects(GameObject gun)
//     {
//         gun.GetComponent<GunFireScript>().gunSoundScript.PlayGunshotSound();
//         gun.GetComponent<GunFireScript>().muzzleFlash.Play();
//     }

//     [ServerRpc]
//     void AlertEnemies(GameObject gun)
//     {
//         Collider[] allEnemiesInEarshot = Physics.OverlapSphere(gun.transform.position, enemyAlertRadius, enemyLayer);

//         Debug.Log(allEnemiesInEarshot.Length);

//         for (int x = 0; x < allEnemiesInEarshot.Length; x++)
//         {
//             allEnemiesInEarshot[x].gameObject.GetComponent<enemyAIPatrol>().HearSound(gun.transform.position);
//         }
//     }

//     void SelectFire()
//     {
//         fullyAutomatic = !fullyAutomatic;
//     }
}

