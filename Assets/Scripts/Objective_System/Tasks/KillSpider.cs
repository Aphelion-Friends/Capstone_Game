using UnityEngine;

public class KillSpider : Task
{
    private string requiredEnemyName = "spider";

    public override void EnemyKilled(string enemyName)
    {
        if (enemyName == requiredEnemyName)
            currentAmount ++;

        checkTaskCompletion();
    }
}
