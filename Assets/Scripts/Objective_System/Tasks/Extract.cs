using UnityEngine;

public struct Extract : Task
{
    public Vector3 extractionLocation;

    private string distanceString;
    private bool _isComplete;
    private float completionDistance;

    public bool isComplete { get => _isComplete; }
    public string displayName { get { return "Escape"; }  }
    public string taskName { get { return "escape"; } }
    public string taskDescription { get { return "Locate the extraction point and escape. Distance: " + distanceString; } }

    public void PlayerMove(Vector3 position)
    {
        float distance = (extractionLocation - position).magnitude;
        distanceString = (Mathf.Round(distance)).ToString();

        if (distance <= completionDistance)
        {
            _isComplete = true;
        }
        else if (distance > completionDistance)
        {
            _isComplete = false;
        }
    }
    public void EnemyKilled(string enemyName) {}
    public void ItemCollected(ItemObject.Name itemName) {}
    public void ItemDropped(ItemObject.Name itemName) {}

    public void Initalize()
    {
        extractionLocation = new Vector3(451, 5, -303);
        completionDistance = 5;
    }
}
