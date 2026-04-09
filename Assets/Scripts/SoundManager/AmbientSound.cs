using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    [SerializeField] float delayedTime;

    [SerializeField][Range(0,1)] float playProbability;

    private enum Soundtype
    {
        metal,
        glass,
        monsterSound,
        areaAmbience
    }

    [SerializeField] Soundtype soundtype;

    private float remainTime;
    private MultiAudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        switch (soundtype)
        {
            case (Soundtype.metal):
                break;

            case (Soundtype.glass):
                break;

            case (Soundtype.monsterSound):
                break;

            case (Soundtype.areaAmbience):
                break;
        }
        audioSource = MultiAudioSource.FromResource(this.gameObject, "labAmbience");

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
