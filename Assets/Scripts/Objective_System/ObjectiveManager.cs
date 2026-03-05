using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using PurrNet.Prediction;
// using UnityEditor.SettingsManagement;
using PurrNet.Modules;
using UnityEngine.SceneManagement;
using System;
using PurrNet;
public class ObjectiveManager : PredictedIdentity<ObjectiveManager.ObjectiveManagerState> {

    public static ObjectiveManager Instance;
    private ObjectiveInitializer objectiveInitializer = new ObjectiveInitializer();

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
            return $"Objective name: {objective.GetType().Name}. Number of tasks: {objective.getNumTasks()}. Is complete? {objective.checkObjectiveCompletion()}. Current task : {objective.GetFirstIncompleteTask().taskName}";
        }
    }

    protected override ObjectiveManagerState GetInitialState()
    {
        Debug.Log("Initalizing objective!");
        return new ObjectiveManagerState
        {
            objective = objectiveInitializer.TestExtract()
        };
    }

    protected override void Simulate(ref ObjectiveManagerState state, float delta)
    {
        
        if (state.objective.isComplete)
        {  
            // var settings = new PurrNet.Modules.PurrSceneSettings();
            // settings.isPublic = false;
            // settings.mode = LoadSceneMode.Additive;
            // predictionManager.networkManager.sceneModule.LoadSceneAsync("TitleScreen");
            // PlayerID playerID = predictionManager.hierarchy.TryGetId
            // predictionManager.networkManager.scenePlayersModule.RemovePlayerFromScene()
            Debug.Log($"PlayerID{predictionManager.networkManager.localPlayer}");
        }
    }

}
