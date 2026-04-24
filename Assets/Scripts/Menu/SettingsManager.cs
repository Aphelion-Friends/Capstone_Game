using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour

{
    [Header("UI References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private float maxVolume = 0.6f;

    private Vector2Int[] customResolutions =
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1600, 900),      //These are some placeholder resolutions, will make it detect the user's resolution and adjust options accordingly later
        new Vector2Int(1280, 720)
    };

    void Start()
    {
        if (volumeSlider == null ||
            resolutionDropdown == null ||
            fullscreenToggle == null)
        {
            Debug.LogError("SettingsManager missing UI references.");
            return;
        }

        SetupResolutionDropdown();
        LoadSettings();
        SetupListeners();
    }

    void SetupListeners()
    {
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        foreach (Vector2Int res in customResolutions)
        {
            options.Add(res.x + " x " + res.y);
        }

        resolutionDropdown.AddOptions(options);
    }

    void LoadSettings()
    {
        //Load saved values from PlayerPrefs
        float savedVolume = Mathf.Clamp(PlayerPrefs.GetFloat("volume", 0.5f), 0f, maxVolume);
        int savedResolution = PlayerPrefs.GetInt("resolution", 0);
        bool savedFullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        //Apply UI values
        volumeSlider.value = savedVolume;
        fullscreenToggle.isOn = savedFullscreen;
        resolutionDropdown.value = savedResolution;
        resolutionDropdown.RefreshShownValue();

        //Apply actual settings
        AudioListener.volume = savedVolume;
        ApplyResolution(savedResolution, savedFullscreen);
    }

    public void SetVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp(volume, 0f, maxVolume);

        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("volume", volume);         //You'll notice that in all the set functions "PlayerPrefs" is referenced, its basically a built in unity
        PlayerPrefs.Save();                             //class meant for storing user settings. You'll notice even in the editor your setting will be the same as how you left them
    }                                                   //Might want to start using an audiomixer later so we can change the ambience and gun volumes seperately

    public void SetResolution(int index)
    {
        ApplyResolution(index, fullscreenToggle.isOn);

        PlayerPrefs.SetInt("resolution", index);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        ApplyResolution(resolutionDropdown.value, isFullscreen);

        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    void ApplyResolution(int index, bool fullscreen)
    {
        int width = customResolutions[index].x;
        int height = customResolutions[index].y;

        Screen.SetResolution(
            width,
            height,
            fullscreen
                ? FullScreenMode.FullScreenWindow
                : FullScreenMode.Windowed
        );
    }
}
