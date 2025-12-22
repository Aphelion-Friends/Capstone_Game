using UnityEngine;

public class SetObjective : MonoBehaviour
{
    public void SetSpiderAss()
    {
        ObjectiveManager.Instance.objective = new CollectSpiderAss();
    }
}
