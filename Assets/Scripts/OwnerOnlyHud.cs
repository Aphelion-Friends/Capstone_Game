using UnityEngine;
using PurrNet.Prediction;

public class OwnerOnlyHUD : StatelessPredictedIdentity
{
    [SerializeField] private GameObject hudRoot;

    protected override void LateAwake()
    {
        if (hudRoot == null)
            hudRoot = gameObject;

        hudRoot.SetActive(isOwner);

        if (!isOwner)
        {
            enabled = false;
        }
    }
}