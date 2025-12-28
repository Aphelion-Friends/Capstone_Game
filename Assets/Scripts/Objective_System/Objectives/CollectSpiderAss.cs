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

        objective.tasks.Add(new KillSpider());
        objective.tasks.Add(new SpiderAss());
        objective.tasks.Add(new Extract());

        return objective;
    }
}
