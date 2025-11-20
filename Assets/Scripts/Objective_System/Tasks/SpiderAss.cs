public class SpiderAss : Task
{
    
    ItemObject.Name taskItem = ItemObject.Name.SpiderAss;
    public override void ItemCollected(ItemObject.Name itemName)
    {
        currentAmount++;

        if (taskItem == itemName)
        {
            currentAmount++;
            checkTaskCompletion();
        }

    }

    public override void ItemDropped(ItemObject.Name itemName)
    {
        if (taskItem == itemName)
        {
            currentAmount--;
            checkTaskCompletion();
        }
    }
}