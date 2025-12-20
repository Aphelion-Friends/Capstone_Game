using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScene : MonoBehaviour
{
    public RawImage FadeOutImage;

    public void OnButtonClick()
    {
        FadeOutImage.gameObject.SetActive(true);
        Invoke("LoadNext", 1f);
    }

    void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
