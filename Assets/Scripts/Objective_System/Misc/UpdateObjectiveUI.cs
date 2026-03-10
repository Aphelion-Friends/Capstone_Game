using UnityEngine;
using TMPro;

public class UpdateObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private TextMeshProUGUI taskDescription;
    private Objective objective;

    void Update()
    {   
        Objective objective = ObjectiveManager.Instance.currentState.objective;

        if (!objective.checkObjectiveCompletion())
        {
            Task currentTask = objective.GetFirstIncompleteTask();
            taskName.text = currentTask.taskName;
            taskDescription.text = currentTask.taskDescription;
            Debug.Log(taskName.text);
        }
        else
        {
            taskName.text = "All Objectives Completed";
            taskDescription.text = "";
        }
    }
}
