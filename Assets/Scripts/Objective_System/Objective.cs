using System.Collections.Generic;
using UnityEngine;
using System;

public struct Objective
{
    public List<Task> tasks;
    public int numTasks;
    public bool isComplete;

    // List of function to call every time the objective is updated
    private List<Action> onChangeList;

    void Start()
    {
        numTasks = tasks.Count;
    }

    public int getNumTasks() {  return numTasks; }

    // Whenever the objective status changes, all the functions in onChangeList are called
    private void OnChange()
    {
        foreach (Action onChangeFunction in onChangeList)
        {
            onChangeFunction();
        }
    }

    public void Subscribe(Action newFunction)
    {
        onChangeList.Add(newFunction);
    }

    public Task GetFirstIncompleteTask()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            if(task.isComplete == false)
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
        OnChange();
    }

    public void EnemyKilled(string enemyName) {
        foreach (Task task in tasks)
        {
            task.EnemyKilled(enemyName);
        }

        checkObjectiveCompletion();
        OnChange();
    }

    public void ItemCollected(ItemObject.Name itemName) {
        foreach (Task task in tasks)
        {
            task.ItemCollected(itemName);
        }

        checkObjectiveCompletion();
        OnChange();
    }

    public void ItemDropped(ItemObject.Name itemName) {
        foreach (Task task in tasks)
        {
            task.ItemDropped(itemName);
        }

        checkObjectiveCompletion();

    }

    public bool checkObjectiveCompletion()
    {
        bool taskIncomplete = false;
        
        foreach (Task task in tasks)
        {
            if(task.checkTaskCompletion() == false)   //Go through lisit of task objects and see if any are incomplete
            {
                taskIncomplete = true;                          //If a task isn't complete, set taskIncomplete to true to prevent the Objective from being marked complete
            }
        }

        if (!taskIncomplete) //If taskComplete is false that means all tasks are marked as complete and thus the objective can be marked as completed
        { 
            isComplete = true;
            Debug.Log("OBJECTIVE COMPLETE!");
        }

        return isComplete;
    }
}
