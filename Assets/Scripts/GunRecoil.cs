using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    private float originalLocalZ;
    [SerializeField] private float recoilAnimationStrength = 3f;
    [SerializeField] private float recoilAnimationTime = 0.1f;

    void Start()
    {
        originalLocalZ = gameObject.transform.localPosition.z;
    }

    public void Recoil(float strength)
    {
        GunFireScript gunFireScript = GetComponent<GunFireScript>();
        LeanTween.moveLocalZ(this.gameObject, originalLocalZ - recoilAnimationStrength, recoilAnimationTime).setEase(LeanTweenType.easeInOutCubic).setDelay(recoilAnimationTime);
        LeanTween.moveLocalZ(this.gameObject, originalLocalZ, recoilAnimationTime).setEase(LeanTweenType.easeInOutCubic).setDelay(recoilAnimationTime);
    }
}
