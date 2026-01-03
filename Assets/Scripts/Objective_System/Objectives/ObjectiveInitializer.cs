using System.Collections.Generic;
using UnityEngine;

public class ObjectiveInitializer
{
    private Objective InitializeGenericObjective()
    {
        Objective objective = new Objective();
        objective.tasks = new List<Task>();


        return objective;
    }

    public Objective CollectSpiderAss()
    {
        Debug.Log("SpiderAss");
        Objective objective = InitializeGenericObjective();

        KillSpider killSpider = new KillSpider();
        SpiderAss spiderAss = new SpiderAss();
        Extract extract = new Extract();

        killSpider.Initalize();
        spiderAss.Initalize();
        extract.Initalize();

        objective.tasks.Add(killSpider);
        objective.tasks.Add(spiderAss);
        objective.tasks.Add(extract);

        return objective;
    }
}
