using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveLobby : MonoBehaviour
{
    public void exitLobby()
    {
        SceneManager.LoadScene(0);
    }
}
