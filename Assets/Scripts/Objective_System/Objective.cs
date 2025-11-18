using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] List<Task> tasks = new List<Task>();
    public int numTasks;
    bool isComplete = false;

    void Start()
    {
        numTasks = tasks.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
