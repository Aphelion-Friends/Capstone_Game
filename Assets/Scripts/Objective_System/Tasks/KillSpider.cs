public class KillSpider : Task
{
    private string requiredEnemyName = "spider";

    public KillSpider()
    {
        taskName = "KillSpider";
        displayName = "Kill Spider";
        taskDescription = "Find and kill a spider.";
    }

    public override void EnemyKilled(string enemyName)
    {
        if (enemyName == requiredEnemyName)
            currentAmount ++;

        checkTaskCompletion();
    }
}
