using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HoldGun : MonoBehaviour
{
    [SerializeField] Transform rightHandTargetPos;
    [SerializeField] TwoBoneIKConstraint rightHandIK;
    [SerializeField] Transform rightHandIKTarget;
    [SerializeField] Transform cameraTransform;
    // [SerializeField] GameObject gun;

    bool holdingGun = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (holdingGun)
        {
            // Debug.Log(rightHandTargetPos.position);
            rightHandIKTarget.rotation = cameraTransform.rotation * Quaternion.Euler(90, 0, 0);
            rightHandIKTarget.position = rightHandTargetPos.position;
            rightHandIK.weight = 1f;
        }
        else
        {
            rightHandIK.weight = 0f;
        }
    }
}
