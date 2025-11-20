using UnityEngine;

public class SpiderAss : Task
{
    public override void ItemCollected(string itemName) 
    {
        currentAmount++;


    }
    public override void ItemDropped(string itemName) { }
}
