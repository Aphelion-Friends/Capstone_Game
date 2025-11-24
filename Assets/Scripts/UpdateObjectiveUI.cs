using UnityEngine;
using TMPro;

public class UpdateObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private TextMeshProUGUI taskDescription;
    
    void Start()
    {
        Objective objective = ObjectiveManager.Instance.objective;
        UpdateTaskNameAndDescription(objective);
    }

    void UpdateTaskNameAndDescription(Objective objective)
    {
        Task currentTask = objective.GetFirstIncompleteTask();
        taskName.text = currentTask.displayName;
        taskDescription.text = currentTask.taskDescription;
    }

    void Update()
    {
        
    }
}
