using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Rigidbody gun;

    public void Recoil(float strength)
    {
        gun.AddForce(transform.forward * -strength);
    }
}
