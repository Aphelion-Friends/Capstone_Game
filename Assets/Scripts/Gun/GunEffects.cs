using UnityEngine;
using UnityEngine.VFX;

public class GunEffects : MonoBehaviour
{
    // [Header("Audio Source")]
    // public AudioSource gunshotAudio;

    // [Header("Settings")]
    // [Range(0f, 1f)] public float volume = 1f;
    // public bool isMuted = false;

    [Header("Visual Effects")]
    [SerializeField] private VisualEffect _muzzleFlash;
    private GunRecoil gunRecoil;

    private MultiAudioSource audioSource;

    void Awake()
    {
        audioSource = MultiAudioSource.FromResource(this.gameObject, "gunshot");
        gunRecoil = GetComponentInChildren<GunRecoil>();
    }
    public void PlayEffects()
    {
        audioSource.Play();
        _muzzleFlash.Play();
        gunRecoil.Recoil();
    }

    // void Start()
    // {
    //     if (gunshotAudio != null)
    //     {
    //         gunshotAudio.volume = volume;
    //         gunshotAudio.mute = isMuted;
    //     }
    // }

    // public void PlayGunshotSound()
    // {
    //     if (gunshotAudio != null && gunshotAudio.clip != null)
    //     {
    //         gunshotAudio.volume = volume; // update to latest
    //         gunshotAudio.mute = isMuted;
    //         gunshotAudio.PlayOneShot(gunshotAudio.clip, volume);
    //     }
    // }

    // public void SetVolume(float newVolume)
    // {
    //     volume = Mathf.Clamp01(newVolume);
    //     if (gunshotAudio != null)
    //         gunshotAudio.volume = volume;
    // }

    // public void ToggleMute()
    // {
    //     isMuted = !isMuted;
    //     if (gunshotAudio != null)
    //         gunshotAudio.mute = isMuted;
    // }

    // public void SetPitch(float newPitch)
    // {
    //     if (gunshotAudio != null)
    //         gunshotAudio.pitch = Mathf.Clamp(newPitch, 0.5f, 2f); // keeps it natural
    // }


}
