using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuToolTip;
    [SerializeField] private GameObject ObjectiveVisual;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private GameObject AmmoCount;

    [Header("Disable While Paused")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private FirstPersonCamera firstPersonCamera;

    private bool isOpen = false;
    private bool wasPausePressed = false;

    public static bool IsPaused { get; private set; }

    private void Start()
    {
        SetPauseState(false);
    }

    private void Update()
    {
        if (InputManager.Instance == null || InputManager.Instance.pauseAction == null)
            return;

        bool pausePressed = InputManager.Instance.pauseAction.inProgress;

        if (pausePressed && !wasPausePressed)
        {
            SetPauseState(!isOpen);
        }

        wasPausePressed = pausePressed;
    }

    private void SetPauseState(bool open)
    {
        isOpen = open;

        IsPaused = isOpen;

        if (pauseMenu != null)
            pauseMenu.SetActive(isOpen);

        if (pauseMenuToolTip != null)
            pauseMenuToolTip.SetActive(!isOpen);

        if (ObjectiveVisual != null)
            ObjectiveVisual.SetActive(!isOpen);

        if (Crosshair != null)
            Crosshair.SetActive(!isOpen);

        if (AmmoCount != null)
            AmmoCount.SetActive(!isOpen);

        if (playerMovement != null)
            playerMovement.enabled = !isOpen;

        if (firstPersonCamera != null)
            firstPersonCamera.enabled = !isOpen;

        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}