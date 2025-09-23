//Most of this script comes from the Brackey Unity Gun tutorial 
// You can check it out here --> https://www.youtube.com/watch?v=THnivyG0Mvo&list=PLPV2KyIb3jR7dFbE2UQYu7QWMdUgDnlnk 

using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GunFireScript : MonoBehaviour
{

    public StarterAssetsInputs input;
    
    public float damage = 10f; //Change these values if you want to make the damage or range bigger or smaller
    public float range = 100f; //Ideally I'd like to have other variables like gun recoil 
    public Camera fpsCam;


    

    void Update()
    {
        if (input.click)
        {
            Shoot();
            input.click = false;
        }
    }

    void Shoot()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
        {
            Debug.Log("Bang!");                         //Everytime you fire this will be send into the debug log
            //Debug.Log(hitInfo.transform.name);        //Optionally you can uncomment this line of code and it'll tell you what you fired at
        }
    }



}

