using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (!Camera.main)
            return;

        transform.forward = Camera.main.transform.forward;
    }
}
