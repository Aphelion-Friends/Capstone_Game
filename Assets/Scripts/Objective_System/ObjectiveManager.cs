using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class ObjectiveManager : MonoBehaviour
{
    List<Objective> objectives = new List<Objective>();

    public static ObjectiveManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
