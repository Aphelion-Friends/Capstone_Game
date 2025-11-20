using UnityEngine;

[System.Serializable]
public abstract class Task
{
    public bool isComplete = false;
    public string displayName;
    public string taskName;
    public string taskDescription;
    public int amount = 1;
    public int currentAmount = 0;

    public virtual void PlayerMove(Vector3 position) {}
    public virtual void EnemyKilled(string enemyName) {}
    public virtual void ItemCollected(string itemName) {}
    public virtual void ItemDropped(string itemName) {}
}
