using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    FirstPersonCamera cameraScript;

    [SerializeField] float recoilX = 1f;
    [SerializeField] float recoilY = 10f;

    void Awake()
    {
        cameraScript = GetComponentInParent<FirstPersonCamera>();
    }

    public void Recoil()
    {
        cameraScript.AddRotation(new Vector2(-recoilY, Random.Range(-recoilX, recoilX)));
    }
}
