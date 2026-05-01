using System.Collections;
using UnityEngine;

public class ObjectiveSlideUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform objectiveRoot;

    [Header("Audio")]
    private MultiAudioSource objectiveAudio;

    [Header("Positions")]
    [SerializeField] private Vector2 hiddenPosition = new Vector2(450f, 0f);
    [SerializeField] private Vector2 visiblePosition = new Vector2(0f, 0f);

    [Header("Timing")]
    [SerializeField] private float slideDuration = 0.35f;
    [SerializeField] private float stayVisibleDuration = 5f;

    private Coroutine slideRoutine;

    private void Awake()
    {
        if (objectiveRoot == null)
            objectiveRoot = GetComponent<RectTransform>();

        objectiveRoot.anchoredPosition = hiddenPosition;

        objectiveAudio = MultiAudioSource.FromResource(gameObject, "ObjectiveSound");
    }

    private void Update()
    {
        if (InputManager.Instance == null || InputManager.Instance.objectiveAction == null)
            return;

        if (InputManager.Instance.objectiveAction.WasPressedThisFrame())
        {
            ShowObjectiveWithSound();
        }
    }

    public void ShowObjectiveWithSound()
    {
        if (objectiveAudio != null)
            objectiveAudio.Play();

        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        slideRoutine = StartCoroutine(SlideSequence());
    }

    private IEnumerator SlideSequence()
    {
        yield return SlideTo(visiblePosition);

        yield return new WaitForSeconds(stayVisibleDuration);

        yield return SlideTo(hiddenPosition);
    }

    private IEnumerator SlideTo(Vector2 targetPosition)
    {
        Vector2 startPosition = objectiveRoot.anchoredPosition;
        float timer = 0f;

        while (timer < slideDuration)
        {
            timer += Time.deltaTime;

            float t = timer / slideDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            objectiveRoot.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        objectiveRoot.anchoredPosition = targetPosition;
    }

    public void ShowObjectiveSilent()
    {
        if (slideRoutine != null)
            StopCoroutine(slideRoutine);

        slideRoutine = StartCoroutine(SlideSequence());
    }
}