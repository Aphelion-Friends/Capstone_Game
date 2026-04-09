using UnityEngine;

public class PlayerOnMove : MonoBehaviour
{
    private Vector3 lastPosition;

    void Update()
    {
        Vector3 currentPosition = gameObject.transform.position;
        if (currentPosition != lastPosition)
        {
            ObjectiveManager.Instance.currentState.objective.PlayerMove(currentPosition);
        }
    }

}
