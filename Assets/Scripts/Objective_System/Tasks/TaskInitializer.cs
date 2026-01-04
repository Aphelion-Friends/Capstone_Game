using UnityEngine;

public class TaskInitializer : MonoBehaviour
{
    private Task InitializeGenericTask()
    {
        Task newTask = new Task();
        newTask.isComplete = false;
        newTask.displayName = "Default Task";
        newTask.taskName = "DefaultTask";
        newTask.taskDescription = "Blah blah blah default value.";
        newTask.originalTaskDescription = newTask.taskDescription;
        newTask.amount = 0;

        newTask.playerMoveEnabled = false;
        newTask.enemyKilledEnabled = false;
        newTask.itemCollectedEnabled = false;
        newTask.itemDroppedEnabled = false;

        return newTask;
    }

    public Task KillSpider()
    {
        Task killSpider = InitializeGenericTask();
        killSpider.taskName = "Kill Spider";
        killSpider.taskDescription = "Find and kill a spider";
        killSpider.enemyKilledEnabled = true;
        killSpider.requiredEnemyName = "spider";

        return killSpider;
    }

    public Task SpiderAss()
    {
        Task spiderAss = InitializeGenericTask();
        spiderAss.taskName = "Collect Spider Remains";
        spiderAss.taskDescription = "Collect the abdomen from the spider you just killed.";
        spiderAss.itemCollectedEnabled = true;
        spiderAss.itemDroppedEnabled = true;
        spiderAss.taskItem = "spiderAss";
        spiderAss.targetAmount = 1;

        return spiderAss;
    }

    public Task Extract()
    {
        Task extract = InitializeGenericTask();
        extract.taskName = "Escape";
        extract.taskDescription = "Locate the exit and escape. Distance: ";
        extract.originalTaskDescription = extract.taskDescription;
        extract.playerMoveEnabled = true;
        extract.extractionLocation = new Vector3(451, 5, -303);
        extract.completionDistance = 5f;

        return extract;
    }
}
