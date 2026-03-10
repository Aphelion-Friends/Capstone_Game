using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
public class PlayerTests
{
    PlayerHealth testHealth;
    PlayerMovement testMovement;
    PlayerShoot testShoot;

    [SetUp]

    public void setUp()
    {
        GameObject testPlayer = new GameObject("TestPlayer");

        testHealth = testPlayer.AddComponent<PlayerHealth>();
        testMovement = testPlayer.AddComponent<PlayerMovement>();
        testShoot = testPlayer.AddComponent<PlayerShoot>();

    }

    [Test]

    public void HealthValueChange()
    {
        testHealth.ChangeHealth(50f); //Increase to 50 from 0
        Assert.AreEqual(testHealth.currentState.health, 50);

        testHealth.ChangeHealth(-50f); //Decrase to 0 from 50
        Assert.AreEqual(testHealth.currentState.health, 0);
    }

    [Test]
    public void ConfirmDeath()
    {
        testHealth.ChangeHealth(-1f); //Player should be marked as dead as soon as value drops below 0
        Assert.AreEqual(testHealth.currentState.isDead, true);
    }
}
