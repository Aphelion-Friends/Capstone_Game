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

            Debug.Log("PAUSE");
            paused = !paused;
            input.pause = false;
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        GetComponent<Canvas>().enabled = true;
        input.locked = true;
    }

    void Resume()
    {
        Time.timeScale = 1;
        GetComponent<Canvas>().enabled = false;
        input.locked = false;
    }
}
