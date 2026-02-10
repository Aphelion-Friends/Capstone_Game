using UnityEngine;

public struct Task
{
    // For location-based tasks
    public Vector3 extractionLocation;
    public float completionDistance;

    // For item-based tasks
    public string taskItem;
    public int amount;
    public int targetAmount;

    // For enemy killing-based tasks
    public string requiredEnemyName;

    // For all tasks
    public bool isComplete;
    public string displayName;
    public string taskName;
    public string taskDescription;
    public string originalTaskDescription;

    // To enable or disable the functions since we can't use inheritance
    public bool playerMoveEnabled;
    public bool enemyKilledEnabled;
    public bool itemCollectedEnabled;
    public bool itemDroppedEnabled;
    public bool powerActivationEnabled;

    public void PlayerMove(Vector3 position)
    {
        if (!playerMoveEnabled)
            return;

        float distance = (extractionLocation - position).magnitude;

        string distanceString = (Mathf.Round(distance)).ToString();
        taskDescription = originalTaskDescription + distanceString;

        if (distance <= completionDistance)
        {
            isComplete = true;
        }
        else if (distance > completionDistance)
        {
            isComplete = false;
        }
    }

    public void EnemyKilled(string enemyName)
    {
        if (!enemyKilledEnabled)
            return;

        if (enemyName == requiredEnemyName)
        {
            Debug.Log("I died!!!!!");
            isComplete = true;
            Debug.Log(isComplete);
        }
    }

    public void ItemCollected(string itemName)
    {
        if (!itemCollectedEnabled)
            return;

        if (itemName == taskItem)
        {
            amount++;
            if (amount >= targetAmount)
            {
                isComplete = true;
            }
        }
    }

    public void ItemDropped(string itemName)
    {
        if (!itemDroppedEnabled)
            return;

        if (itemName == taskItem)
        {
            amount--;
            
            if (amount < targetAmount)
            {
                isComplete = false;
            }
        }
    }

    public void ActivatePower()
    {
        if(!powerActivationEnabled)
            return;

        isComplete = true;
    }


}
