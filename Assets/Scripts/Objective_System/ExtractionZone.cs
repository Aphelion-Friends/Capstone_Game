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
        if(!other.TryGetComponent(out PlayerHealth player))
        {
            return;
        }

        ObjectiveManager.Instance.currentState.objective.ExtractTouched();
    }
}
