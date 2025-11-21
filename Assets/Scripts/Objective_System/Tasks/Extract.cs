using UnityEngine;

public class Extract : Task
{
    private Vector3 extractionLocation;
    private float completionDistance;

    public override void PlayerMove(Vector3 position)
    {
        float distance = (extractionLocation - position).magnitude;

        if (distance <= completionDistance)
        {
            currentAmount++;
        }
    }
}
