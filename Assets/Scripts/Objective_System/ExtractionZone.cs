using UnityEngine;
using PurrNet.Prediction;

[RequireComponent(typeof(PredictedRigidbody))]
public class ExtractionZone : StatelessPredictedIdentity
{
    private PredictedRigidbody extract;
    protected override void LateAwake()
    {
        extract = GetComponent<PredictedRigidbody>();

        extract.onTriggerEnter += OnExtract;
    }

    private void OnExtract(GameObject other)
    {
        if (TryGetComponent(out PlayerHealth player))

        {
            Debug.Log("Playerhealth found");
            player.gameObject.SetActive(false);
            ObjectiveManager.Instance.currentState.objective.ExtractTouched();

            if (ObjectiveManager.Instance.currentState.objective.checkObjectiveCompletion())
            {
                Debug.Log("Objective complete!");
                predictionManager.hierarchy.Delete(other);
            }
        }
        else
        {
            return;
        }

        
    }
}
