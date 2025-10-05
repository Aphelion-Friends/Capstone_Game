using UnityEngine;
using UnityEngine.InputSystem; 

public class FlashlightToggle : MonoBehaviour
{
    private Light lightComp;

    void Start()
    {
        lightComp = GetComponent<Light>();
    }

    void Update()
    {
        // Check if the 'F' key was pressed this frame
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            lightComp.enabled = !lightComp.enabled;
        }
    }
}

