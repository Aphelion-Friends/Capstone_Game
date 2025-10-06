using UnityEngine;
using StarterAssets;

public class PauseGame : MonoBehaviour
{

    public StarterAssetsInputs input;
    bool paused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (input.pause)
        {
            if (paused) Resume();
            if (!paused) Pause();

            input.pause = false;
        }
    }

    public void Pause()
    {
        input.cursorLocked = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        GetComponent<Canvas>().enabled = true;
        input.locked = true;
        paused = !paused;
    }

    public void Resume()
    {
        input.cursorLocked = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<Canvas>().enabled = false;
        input.locked = false;
        paused = !paused;
    }
}
