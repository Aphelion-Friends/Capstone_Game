using System.Collections.Generic;
using UnityEngine;

public class Objective
{
    protected List<Task> tasks = new List<Task>();
    private int numTasks;
    bool isComplete = false;

    void Start()
    {
        numTasks = tasks.Count;
    }

    public int getNumTasks() {  return numTasks; }

    public Task GetFirstIncompleteTask()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            if (task.isComplete)
            {
                return task;
            }
        }
        return null;
    }

    public void PlayerMove(Vector3 position)
    {
        foreach (Task task in tasks)
        {
            task.PlayerMove(position);
        }

        checkObjectiveCompletion();

    }

    public void EnemyKilled(string enemyName) {
        foreach (Task task in tasks)
        {
            task.EnemyKilled(enemyName);
        }

        checkObjectiveCompletion();

    }

    public void ItemCollected(ItemObject.Name itemName) {
        foreach (Task task in tasks)
        {
            task.ItemCollected(itemName);
        }

        checkObjectiveCompletion();

    }

    public void ItemDropped(ItemObject.Name itemName) {
        foreach (Task task in tasks)
        {
            task.ItemDropped(itemName);
        }

        checkObjectiveCompletion();

    }

    public void checkObjectiveCompletion()
    {
        bool taskIncomplete = false;
        
        foreach (Task task in tasks)
        {
            if(task.isComplete == false)   //Go through lisit of task objects and see if any are incomplete
            {
                taskIncomplete = true;                          //If a task isn't complete, set taskIncomplete to true to prevent the Objective from being marked complete
            }
        }

        if (taskIncomplete) //If taskComplete is false that means all tasks are marked as complete and thus the objective can be marked as completed
        { 
            isComplete = true;
            Debug.Log("OBJECTIVE COMPLETE!");
        }
    }
}
