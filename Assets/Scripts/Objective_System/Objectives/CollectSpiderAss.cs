using System.Collections.Generic;

public class ObjectiveInitalizer
{
    private Objective InitalizeGenericObjective()
    {
        Objective objective = new Objective();
        objective.tasks = new List<Task>();


        return objective;
    }

    public Objective CollectSpiderAss()
    {
        Objective objective = new Objective();

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
