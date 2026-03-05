using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using PurrLobby;
using UnityEngine.SceneManagement;
using PurrNet;
using PurrNet.Prediction;

public struct Objective
{
    public List<Task> tasks;
    public int numTasks;
    public bool isComplete;

    // List of function to call every time the objective is updated

    public int getNumTasks() {  return numTasks; }

    // Whenever the objective status changes, all the functions in onChangeList are called
    //private void OnChange()
    //{
        
    //    foreach (Action onChangeFunction in onChangeList)
    //    {
    //        onChangeFunction();
    //    }
    //}

    //public void Subscribe(Action newFunction)
    //{
    //    onChangeList.Add(newFunction);
    //}

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
        return new Task();
    }

    public void PlayerMove(Vector3 position)
    {
        foreach (Task task in tasks)
        {
            task.PlayerMove(position);
        }

        checkObjectiveCompletion();
        //OnChange();
    }

    public void EnemyKilled(string enemyName)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            Task newTask = tasks[i];
            newTask.EnemyKilled(enemyName);
            tasks[i] = newTask;
        }

        checkObjectiveCompletion();
        //OnChange();
    }

    public void ItemCollected(string itemName) {
        for (int i = 0; i < tasks.Count; i++)
        {
            Task newTask = tasks[i];
            newTask.ItemCollected(itemName);
            tasks[i] = newTask;
        }

        checkObjectiveCompletion();
        //OnChange();
    }

    public void ItemDropped(string itemName) {
        foreach (Task task in tasks)
        {
            task.ItemDropped(itemName);
        }

        checkObjectiveCompletion();

    }

    public bool checkObjectiveCompletion()
    {
        bool taskIncomplete = false;
        Debug.Log(tasks[0].taskName + " is " + tasks[0].isComplete);

        foreach (Task task in tasks)
        {

            Debug.Log(task.taskName + " is " + task.isComplete);
            
            if (!task.isComplete)   //Go through list of task objects and see if any are incomplete
            {
                taskIncomplete = true;                          //If a task isn't complete, set taskIncomplete to true to prevent the Objective from being marked complete
            }
            else
            {
                Debug.Log(task.taskName + "Checked off");
            }
        }

        if (!taskIncomplete) //If taskComplete is false that means all tasks are marked as complete and thus the objective can be marked as completed
        { 
            isComplete = true;
            Debug.Log("OBJECTIVE COMPLETE!");
        }

        return isComplete;
    }

    public void ExtractTouched()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            Task newTask = tasks[i];
            newTask.ExtractTouched();
            tasks[i] = newTask;
        }

        if (checkObjectiveCompletion())
        {
            Debug.Log("Extract Touched");
            // SceneManager.LoadScene("TitleScreen");
        }
    }

}
