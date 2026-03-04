using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class InventoryUITests
{
    private GameObject go;
    private InventoryUI ui;

    [SetUp]
    public void setUp()
    {
        InventoryUI.inventoryOpen = false;
        go = new GameObject("InventoryUI_Test");
        ui = go.AddComponent<InventoryUI>();

        InvokeNonPublic(ui, "Awake");
        InvokeNonPublic(ui, "OnEnable");
    }

    [TearDown]
    public void tearDown()
    {
        Object.DestroyImmediate(go);
        InventoryUI.inventoryOpen = false;
    }

    [Test]
    public void ToggleInventory()
    {
        var cg = go.GetComponent<CanvasGroup>();
        Assert.IsNotNull(cg, "CanvasGroup was not created by Awake.");

        InvokeNonPublic(ui, "ToggleInventory");

        Assert.IsTrue(InventoryUI.inventoryOpen);
        Assert.AreEqual(1f, cg.alpha);
        Assert.IsTrue(cg.interactable);
        Assert.IsTrue(cg.blocksRaycasts);

        InvokeNonPublic(ui, "ToggleInventory");

        Assert.IsFalse(InventoryUI.inventoryOpen);
        Assert.AreEqual(0f, cg.alpha);
        Assert.IsFalse(cg.interactable);
        Assert.IsFalse(cg.blocksRaycasts);
    }

    private static void InvokeNonPublic(object target, string methodName)
    {
        var m = target.GetType().GetMethod(methodName,
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        Assert.IsNotNull(m, $"Could not find method '{methodName}' on {target.GetType().Name}.");
        m.Invoke(target, null);
    }
}