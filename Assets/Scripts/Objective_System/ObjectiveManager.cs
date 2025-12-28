using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using PurrNet.Prediction;
public class ObjectiveManager : PredictedIdentity<ObjectiveManager.ObjectiveManagerState> {

    public static ObjectiveManager Instance;
    private ObjectiveInitalizer objectiveInitalizer = new ObjectiveInitalizer();

    private void Awake()
    {
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    public struct ObjectiveManagerState : IPredictedData<ObjectiveManagerState> 
    {
        public Objective objective;
        public void Dispose() { }

        public override string ToString()
        {
            return $"Objective name: {objective.GetType().Name}. Number of tasks: {objective.getNumTasks()}. Is complete? {objective.checkObjectiveCompletion()}";
        }
    }

    protected override ObjectiveManagerState GetInitialState()
    {
        Debug.Log("Initalizing objective!");
        return new ObjectiveManagerState
        {
            objective = objectiveInitalizer.CollectSpiderAss(),
        };
    }

}
