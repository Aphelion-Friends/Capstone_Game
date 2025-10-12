using UnityEngine;
using UnityEngine.InputSystem;
using PurrNet;

public class FlashlightToggle : NetworkIdentity 
{
    private Light lightComp;
    private SyncVar<bool> lightOn = new(false, ownerAuth:true);

    protected override void OnSpawned()
    {
        lightComp = GetComponent<Light>();
        lightOn.onChanged += SetLight;
        SetLight(false);
    }

    protected override void OnDestroy()
    {
        lightOn.onChanged -= SetLight;
    }

    private void ToggleLight()
    {
        if (isOwner)
            lightOn.value = !lightOn.value;
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

