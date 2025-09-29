using UnityEngine;

public class AdjustGunPosition : MonoBehaviour
{
    public Vector3 relativeGunOffset;
    public Transform gunTransform;
    public Rigidbody gunRigidbody;
    public Transform cameraTransform;

    public float proportionalGain;
    public float integralGain;
    public float derivativeGain;
    public float forceMultiplier;

    public float proportionalGainTorque;
    public float integralGainTorque;
    public float derivativeGainTorque;
    public float torqueMultiplier;

    PIDController PIDx = new PIDController();
    PIDController PIDy = new PIDController();
    PIDController PIDz = new PIDController();
    Vector3 linearTarget;

    PIDController PIDxRot = new PIDController();
    PIDController PIDyRot = new PIDController();
    PIDController PIDzRot = new PIDController();
    Vector3 targetRotation;

    PIDController[] allLinearPIDs = new PIDController[3];
    PIDController[] allRotationalPIDs = new PIDController[3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(Mathf.DeltaAngle(2, 359));

        allLinearPIDs[0] = PIDx;
        allLinearPIDs[1] = PIDy;
        allLinearPIDs[2] = PIDz;

        allRotationalPIDs[0] = PIDxRot;
        allRotationalPIDs[1] = PIDyRot;
        allRotationalPIDs[2] = PIDzRot;

        for(int x = 0; x < 3; x++)
        {
            allLinearPIDs[x].proportionalGain = proportionalGain;
            allLinearPIDs[x].integralGain = integralGain;
            allLinearPIDs[x].derivativeGain = derivativeGain;

            allRotationalPIDs[x].proportionalGain = proportionalGainTorque;
            allRotationalPIDs[x].integralGain = integralGainTorque;
            allRotationalPIDs[x].derivativeGain = derivativeGainTorque;
        }
    }

    void FixedUpdate()
    {
        Vector3 linearTarget = relativeGunOffset.x * cameraTransform.forward + relativeGunOffset.y * cameraTransform.up + relativeGunOffset.z * cameraTransform.right;
        linearTarget += cameraTransform.position;

        Vector3 rotationalTarget = cameraTransform.rotation.eulerAngles;

        // Debug.Log("rotation: " + rotationalTarget);
        // Debug.Log("current rotation: " + gunTransform.eulerAngles);

        float forceX = PIDx.Update(Time.fixedDeltaTime, gunTransform.position.x, linearTarget.x);
        float forceY = PIDy.Update(Time.fixedDeltaTime, gunTransform.position.y, linearTarget.y);
        float forceZ = PIDz.Update(Time.fixedDeltaTime, gunTransform.position.z, linearTarget.z);

        Vector3 totalForce = new Vector3(forceX, forceY, forceZ) * forceMultiplier;

        Vector3 gunRotation = Vector3.Slerp(gunRigidbody.rotation.eulerAngles, rotationalTarget, 0.2f);

        gunRigidbody.AddForce(totalForce);
        //gunRigidbody.AddTorque(totalTorque);
        Quaternion gunRotationQ = new Quaternion();
        gunRotationQ.eulerAngles = gunRotation;

        gunRigidbody.rotation = gunRotationQ;
    }
}
