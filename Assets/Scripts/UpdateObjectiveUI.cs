using UnityEngine;
using TMPro;

public class UpdateObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private TextMeshProUGUI taskDescription;
    
    void Start()
    {
        Objective objective = ObjectiveManager.Instance.objective;
        UpdateTaskNameAndDescription();

        objective.Subscribe(UpdateTaskNameAndDescription);
    }

    void UpdateTaskNameAndDescription()
    {
        Objective objective = ObjectiveManager.Instance.objective;

        if (!objective.checkObjectiveCompletion())
        {
            Task currentTask = objective.GetFirstIncompleteTask();
            taskName.text = currentTask.displayName;
            taskDescription.text = currentTask.taskDescription;
        }
        else
        {
            taskName.text = "All Objectives Completed";
            taskDescription.text = "";
        }
    }
}
