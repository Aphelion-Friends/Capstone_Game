using UnityEngine;

public interface Task
{
    public bool isComplete { get; }
    public string displayName { get; }
    public string taskName { get; }
    public string taskDescription { get; }

    public void PlayerMove(Vector3 position) {}
    public void EnemyKilled(string enemyName) {}
    public void ItemCollected(ItemObject.Name itemName) {}
    public void ItemDropped(ItemObject.Name itemName) {}

    public void Initalize() {}
}
