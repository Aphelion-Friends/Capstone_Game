using UnityEngine;

public class DisableButton : MonoBehaviour
{
    public UnityEngine.UI.Button button;
    void Start()
    {
        button.interactable = false;
    }

}
