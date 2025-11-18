using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] List<GameObject> tasks = new List<GameObject>();
    private int numTasks;
    bool isComplete = false;

    void Start()
    {
        numTasks = tasks.Count;
    }

    public int getNumTasks() {  return numTasks; }

    public void checkTaskCompletion()
    {
        bool taskIncomplete = false;
        
        foreach (GameObject task in tasks)
        {
            if(task.GetComponent<Task>().isComplete == false)   //Go through lisit of task objects and see if any are incomplete
            {
                taskIncomplete = true;                          //If a task isn't complete, set taskIncomplete to true to prevent the Objective from being marked complete
            }
        }

        if (taskIncomplete) //If taskComplete is false that means all tasks are marked as complete and thus the objective can be marked as completed
        { 
            isComplete = true;      
        }
    }

    public void reportTask(string reportedTask)
    {
        foreach (GameObject task in tasks)      //Go through the list of task objects and determine if the reported task matches any in the list 
        {
            if (task.GetComponent<Task>().taskName == reportedTask)
            {
                task.GetComponent<Task>().isComplete = true;    //If it does, set that task to be complete
            }
        }

        checkTaskCompletion();  //Check the status of all tasks afterwards to determine if the objective has been completed

    }
}
