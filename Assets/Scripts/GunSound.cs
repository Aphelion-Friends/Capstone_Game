using UnityEngine;

public class GunSound : MonoBehaviour
{
    public AudioSource gunshotAudio;

    
    public void playGunshotSound()
    {
        gunshotAudio.PlayOneShot(gunshotAudio.clip);
    }
}
