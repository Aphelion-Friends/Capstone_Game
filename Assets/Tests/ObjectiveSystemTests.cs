using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectiveSystemTests
{
    ObjectiveInitializer objectiveInitializer;

    [SetUp]
    public void setUp()
    {
        objectiveInitializer = new ObjectiveInitializer();
    }

    // A Test behaves as an ordinary method
    [Test]
    public void InitializeSpiderAssObjective()
    {
        Objective spiderAss = objectiveInitializer.CollectSpiderAss();

        Assert.AreEqual(spiderAss.tasks.Count, 3);
        Assert.AreEqual(spiderAss.tasks[0].requiredEnemyName, "spider");
        Assert.IsTrue(spiderAss.tasks[0].enemyKilledEnabled);
    }

    [Test]
    public void SpiderAssObjectiveFirstTaskCompletion()
    {
        Objective spiderAss = objectiveInitializer.CollectSpiderAss();
        int killEnemyIndex = 0;

        foreach (Task task in spiderAss.tasks)
        {
            Assert.IsFalse(task.isComplete);
        }

        spiderAss.EnemyKilled("spider");

        for(int i = 0; i < spiderAss.tasks.Count; i++)
        {
            if (i == killEnemyIndex)
            {
                Assert.IsTrue(spiderAss.tasks[i].isComplete);
            }
            else
            {
                Assert.IsFalse(spiderAss.tasks[i].isComplete);
            }
        }
    }

    [Test]
    public void SpiderAssObjectiveSecondTaskCompletion()
    {
        Objective spiderAss = objectiveInitializer.CollectSpiderAss();
        int collectSpiderAssIndex = 1;

        foreach (Task task in spiderAss.tasks)
        {
            Assert.IsFalse(task.isComplete);
        }

        spiderAss.ItemCollected("spiderAss");
        
        for(int i = 0; i < spiderAss.tasks.Count; i++)
        {
            if (i == collectSpiderAssIndex)
            {
                Assert.IsTrue(spiderAss.tasks[i].isComplete);
            }
            else
            {
                Assert.IsFalse(spiderAss.tasks[i].isComplete);
            }
        }
    }
}
