using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Rigidbody gun;
    public Transform cameraTransform;

    public void Recoil(float strength)
    {
        gun.AddForce(cameraTransform.forward * -strength);
    }
}
