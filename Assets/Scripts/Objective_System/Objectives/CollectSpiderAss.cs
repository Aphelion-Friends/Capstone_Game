public class CollectSpiderAss : Objective
{
    public CollectSpiderAss()
    {
        tasks.Add(new KillSpider());
        tasks.Add(new SpiderAss());
        //tasks.Add(new Extract());
    }
}
