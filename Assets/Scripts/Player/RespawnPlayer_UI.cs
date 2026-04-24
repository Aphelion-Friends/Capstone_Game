using UnityEngine;

public class RespawnPlayer_UI : MonoBehaviour
{

    public static RespawnPlayer_UI Instance;

    [SerializeField] private GameObject deathScreen;


    void Awake()
    {
        Instance = this;
    }


    public void ShowUI(bool isOwner)
    {
        if (isOwner)
        {
            RespawnPlayer_UI.Instance.deathScreen.SetActive(true);
        }
        
    } 
}
