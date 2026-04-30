using UnityEngine;
using TMPro;

public class UpdateObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private TextMeshProUGUI taskDescription;

    [SerializeField] private ObjectiveSlideUI objectiveSlideUI;

    private string lastTaskName = "";
    private string lastTaskDescription = "";
    private bool hasInitialized = false;

    private void Awake()
    {
        if (objectiveSlideUI == null)
            objectiveSlideUI = GetComponentInParent<ObjectiveSlideUI>();

        if (objectiveSlideUI == null)
            objectiveSlideUI = FindFirstObjectByType<ObjectiveSlideUI>();
    }

    void Update()
    {
        if (ObjectiveManager.Instance == null)
            return;

        Objective objective = ObjectiveManager.Instance.currentState.objective;

        string newName;
        string newDesc;

        if (!objective.checkObjectiveCompletion())
        {
            Task currentTask = objective.GetFirstIncompleteTask();
            newName = currentTask.taskName;
            newDesc = currentTask.taskDescription;
        }
        else
        {
            newName = "All Objectives Completed";
            newDesc = "";
        }

        taskName.text = newName;
        taskDescription.text = newDesc;

        if (!hasInitialized)
        {
            lastTaskName = newName;
            lastTaskDescription = newDesc;
            hasInitialized = true;

            if (objectiveSlideUI != null)
                objectiveSlideUI.ShowObjectiveSilent();

            return;
        }

        if (newName != lastTaskName || newDesc != lastTaskDescription)
        {
            lastTaskName = newName;
            lastTaskDescription = newDesc;

            if (objectiveSlideUI != null)
                objectiveSlideUI.ShowObjectiveWithSound();
        }
    }
}