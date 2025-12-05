using PurrNet;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PurrLobby
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private LobbyManager lobbyManager;
        [PurrScene, SerializeField] private string nextScene;
        [SerializeField] private PlayerCounter playerCounter;

        public void SwitchScene()
        {
            if (playerCounter.playerCount >= 2)
            {
                lobbyManager.SetLobbyStarted();
                SceneManager.LoadSceneAsync(nextScene);
            }
        }
    }
}
