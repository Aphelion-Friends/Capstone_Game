using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectiveInitializer
{
    TaskInitializer taskInitializer = new TaskInitializer();

    private Objective InitializeGenericObjective()
    {
        Objective objective = new Objective();
        objective.tasks = new List<Task>();
       // objective.onChangeList = new List<Action>();

        return objective;
    }

    public Objective CollectSpiderAss()
    {
        Debug.Log("SpiderAss");
        Objective objective = InitializeGenericObjective();

        Task killSpider = taskInitializer.KillSpider();
        Task spiderAss = taskInitializer.SpiderAss();
        Task extract = taskInitializer.Extract();

        objective.tasks.Add(killSpider);
        objective.tasks.Add(spiderAss);
        objective.tasks.Add(extract);

        objective.numTasks = 3;

        return objective;
    }
}
