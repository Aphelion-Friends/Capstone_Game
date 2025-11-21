using UnityEngine;

public class Extract : Task
{
    private Vector3 extractionLocation;
    private float completionDistance;
    private string originalDescription = "Locate the extraction point and escape. Distance: ";
    private bool inRange = false;

    public Extract()
    {
        taskName = "Extract";
        displayName = "Escape";
        taskDescription = originalDescription;
    }

    public override void PlayerMove(Vector3 position)
    {
        float distance = (extractionLocation - position).magnitude;
        taskDescription = originalDescription + (Mathf.Round(distance)).ToString();

        if (distance <= completionDistance && !inRange)
        {
            currentAmount++;
            inRange = true;
        }
        else if (distance > completionDistance && inRange)
        {
            currentAmount--;
            inRange = false;
        }
    }
}
