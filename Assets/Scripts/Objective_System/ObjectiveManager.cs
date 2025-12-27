using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using PurrNet.Prediction;
public class ObjectiveManager : PredictedIdentity<ObjectiveManager.ObjectiveManagerState> {

    public static ObjectiveManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public struct ObjectiveManagerState : IPredictedData<ObjectiveManagerState> 
    {
        public Objective objective;
        public void Dispose() { }

    }

    protected override ObjectiveManagerState GetInitialState()
    {
        return new ObjectiveManagerState
        {
            objective = new CollectSpiderAss(),
        };
    }

}
