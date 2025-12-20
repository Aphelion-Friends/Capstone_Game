using UnityEngine;

public class UpdateHUD : MonoBehaviour
{
    public TMPro.TMP_Text ammoCountText;
    int ammoCount = 0;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammoCountText.text = ammoCount.ToString();
    }

    public void SetAmmo(int ammo)
    {
        ammoCount = ammo;
    }
}
