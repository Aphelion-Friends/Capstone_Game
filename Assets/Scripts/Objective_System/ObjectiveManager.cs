using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class ObjectiveManager : MonoBehaviour
{
    List<Objective> objectives = new List<Objective>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void addObjective(Objective obj)
    {
        objectives.Add(obj);
    }

    public void reportToObjectives(string taskName)
    {
        foreach (Objective obj in objectives) { }
    }


}
