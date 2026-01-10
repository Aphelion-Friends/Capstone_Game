using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    [SerializeField] float delayedTime;

    [SerializeField][Range(0,1)] float playProbability;

    private float remainTime;
    private MultiAudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = MultiAudioSource.FromResource(this.gameObject, "gunshot");

        remainTime = delayedTime;
    }

    // Update is called once per frame
    void Update()
    {
        remainTime -= Time.deltaTime;
        
        if(remainTime <= 0)
        {
            if(Random.Range(0f,1f) <= playProbability)
                audioSource.Play();

            remainTime = delayedTime;
        }

    }
}
