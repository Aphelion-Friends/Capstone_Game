using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtractionZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collsion)
    {
        Player player = collsion.GetComponent<Player>();

        if (player != null) 
        {
            if (ObjectiveManager.Instance.currentState.objective.checkObjectiveCompletion())
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
