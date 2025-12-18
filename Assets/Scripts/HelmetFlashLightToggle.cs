using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightToggle : MonoBehaviour
{
    private Light lightComp;

    private void ToggleLight()
    {
    }
    
    private void SetLight(bool On)
    {
        lightComp.enabled = On;
    }

    void Update()
    {
        // Check if the 'F' key was pressed this frame
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            ToggleLight(); 
        }
    }
}

