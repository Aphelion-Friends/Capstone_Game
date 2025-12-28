using UnityEngine;
public struct KillSpider : Task
{
    private bool _isComplete;
    private string requiredEnemyName;

    public bool isComplete { get =>_isComplete; }
    public string displayName { get => "Kill Spider"; }
    public string taskName { get => "KillSpider"; }
    public string taskDescription { get => "Find and kill a spider"; }

    public void PlayerMove(Vector3 position) {}
    public void EnemyKilled(string enemyName) {
        if (enemyName == requiredEnemyName)
        {
            _isComplete = true;
        }
    }
    public void ItemCollected(ItemObject.Name itemName) {}
    public void ItemDropped(ItemObject.Name itemName) {}

    public void Initalize() {
        _isComplete = false;
    }
}
