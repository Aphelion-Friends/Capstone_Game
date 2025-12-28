using UnityEngine;

public struct SpiderAss : Task
{
    private bool _isComplete;
    private string taskItem;
    private int currentAmount;

    public bool isComplete { get => _isComplete; }
    public string displayName { get => "Collect Spider Ass"; }
    public string taskName { get => "SpiderAss"; }
    public string taskDescription { get => "Obtain the ass from the spider you just killed."; }

    public void PlayerMove(Vector3 position) {}
    public void EnemyKilled(string enemyName) {}
    public void ItemCollected(string itemName) {
        if (itemName == taskItem)
        {
            currentAmount++;
            if (currentAmount > 0)
            {
                _isComplete = true;
            }
        }
    }
    public void ItemDropped(string itemName) {
        if (itemName == taskItem)
        {
            currentAmount--;
            
            if (currentAmount < 1)
            {
                _isComplete = false;
            }
        }
    }

    public void Initalize() {
        _isComplete = false;
        currentAmount = 0;
        taskItem = "spiderAss";
    }
}
