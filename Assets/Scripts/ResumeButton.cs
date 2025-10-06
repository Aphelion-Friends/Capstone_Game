using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    [SerializeField] PauseGame pauseGame;

    void Start ()
    {
    }

    void OnClick()
    {
        Debug.Log("Test");
        pauseGame.Resume();
    }
}
