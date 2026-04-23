using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonPressHandler : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler,IPointerEnterHandler
{
    [SerializeField] private RectTransform visualRoot;
    [SerializeField] private Vector2 pressedOffset = new Vector2(0f, -4f);

    [Header("Hover Dimming")]
    [SerializeField] private float hoverDimAmount = 0.85f;

    [Header("Audio Settings")]
    [SerializeField] private string pressSoundName = "PressSound";
    //[SerializeField] private string releaseSoundName = "ReleaseSound";
    [SerializeField] private string hoverSoundName = "HoverSound";
    [SerializeField] private float pressVolume = 0.5f;
    [SerializeField] private float releaseVolume = 0.5f;

    private Vector2 _startPos;
    private bool _isHeldDown;
    private bool _isHovering;

    private Graphic[] _graphics;
    private Color[] _originalColors;

    private static GameObject audioObject;
    private static MultiAudioSource pressAudio;
    //private static MultiAudioSource releaseAudio;
    private static MultiAudioSource hoverAudio;

    private void Awake()
    {
        if (!visualRoot)
        {
            visualRoot = (RectTransform)transform;
        }

        _startPos = visualRoot.anchoredPosition;

        _graphics = GetComponentsInChildren<Graphic>();
        _originalColors = new Color[_graphics.Length];

        for (int i = 0; i < _graphics.Length; i++)
        {
            _originalColors[i] = _graphics[i].color;
        }

        SetupAudio();
    }

    private void SetupAudio()
    {
        if (audioObject != null)
            return;

        audioObject = new GameObject("Persistent UI Audio");
        DontDestroyOnLoad(audioObject);

        pressAudio = MultiAudioSource.FromResource(audioObject, pressSoundName);
        pressAudio.SetVolume(pressVolume);

        hoverAudio = MultiAudioSource.FromResource(audioObject, hoverSoundName);
        hoverAudio.SetVolume(pressVolume);

        //releaseAudio = MultiAudioSource.FromResource(audioObject, releaseSoundName);
        //releaseAudio.SetVolume(releaseVolume);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
        SetDimmed(true);

        if (hoverAudio != null)
        {
            hoverAudio.Play();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHeldDown = true;
        visualRoot.anchoredPosition = _startPos + pressedOffset;

        if (pressAudio != null)
        {
            pressAudio.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHeldDown = false;
        visualRoot.anchoredPosition = _startPos;

        //if (releaseAudio != null)
        //{
        //    releaseAudio.PlayOnlyIfDone();
        //}

        SetDimmed(_isHovering);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;

        if (!_isHeldDown)
        {
            visualRoot.anchoredPosition = _startPos;
        }

        SetDimmed(false);
    }

    private void OnDisable()
    {
        _isHeldDown = false;
        _isHovering = false;

        if (visualRoot)
        {
            visualRoot.anchoredPosition = _startPos;
        }

        SetDimmed(false);
    }

    private void SetDimmed(bool dimmed)
    {
        if (_graphics == null || _originalColors == null)
            return;

        for (int i = 0; i < _graphics.Length; i++)
        {
            if (_graphics[i] == null)
                continue;

            _graphics[i].color = dimmed
                ? _originalColors[i] * hoverDimAmount
                : _originalColors[i];
        }
    }
}