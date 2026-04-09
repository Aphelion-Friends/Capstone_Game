using UnityEngine;

public class SpiderCrawl : MonoBehaviour
{

    private MultiAudioSource audioSource;


    void Awake()
    {
        audioSource = MultiAudioSource.FromResource(this.gameObject, "Spidercrawl");
    }
    void Update()
    {
        audioSource.PlayOnlyIfDone();
    }
}
