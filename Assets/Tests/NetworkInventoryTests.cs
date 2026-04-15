using NUnit.Framework;
using UnityEngine;

public class NetworkInventoryTests
{
    private GameObject go;
    private NetworkInventory inv;

    [SetUp]
    public void setUp()
    {
        go = new GameObject("NetworkInventory_Test");
        inv = go.AddComponent<NetworkInventory>();

        inv.EditorInitForTests(slotCount: 24);

        Assert.Greater(inv.SlotCount, 0, "NetworkInventory test init failed (SlotCount == 0).");
    }

    [TearDown]
    public void tearDown()
    {
        Object.DestroyImmediate(go);
    }

    [Test]
    public void AddingItemToInventory_PutsItemInFirstSlot()
    {
        inv.AddItem(itemId: 10, amount: 3);

        Assert.AreEqual(10, inv.GetItemId(0));
        Assert.AreEqual(3, inv.GetAmount(0));
        Assert.IsFalse(inv.IsEmpty(0));
    }

    [Test]
    public void AddingSameItemTwice_StacksItemAmount()
    {
        inv.AddItem(itemId: 10, amount: 3);
        inv.AddItem(itemId: 10, amount: 2);

        Assert.AreEqual(10, inv.GetItemId(0));
        Assert.AreEqual(5, inv.GetAmount(0));
        Assert.IsFalse(inv.IsEmpty(0));
    }

    [Test]
    public void CheckingInvalidInventorySlot()
    {
        Assert.IsTrue(inv.IsEmpty(-1));
        Assert.IsTrue(inv.IsEmpty(inv.SlotCount));
        Assert.IsTrue(inv.IsEmpty(inv.SlotCount + 5));
    }

    [Test]
    public void InvalidInventorySlot_ReturnsNoItemAndZero()
    {
        Assert.AreEqual(0, inv.GetItemId(-1));
        Assert.AreEqual(0, inv.GetItemId(inv.SlotCount));

        Assert.AreEqual(0, inv.GetAmount(-1));
        Assert.AreEqual(0, inv.GetAmount(inv.SlotCount));
    }
}
