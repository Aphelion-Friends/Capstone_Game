using NUnit.Framework.Interfaces;
using UnityEngine;

[System.Serializable]
public abstract class Task
{
    public bool isComplete = false;
    public string displayName;
    public string taskName;
    public string taskDescription;
    protected int amount = 1;
    public int currentAmount = 0;

    public virtual void PlayerMove(Vector3 position) {}
    public virtual void EnemyKilled(string enemyName) {}
    public virtual void ItemCollected(ItemObject.Name itemName) {}
    public virtual void ItemDropped(ItemObject.Name itemName) {}
    
    public bool checkTaskCompletion()
    {
        if(currentAmount >= amount)
        {
            isComplete = true;
        
        }
        else
        {
            isComplete = false;
        }
        
        return isComplete;
    }
}
