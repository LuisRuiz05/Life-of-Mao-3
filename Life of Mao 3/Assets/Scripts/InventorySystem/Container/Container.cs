using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class acts as a slot holder.
/// </summary>
public class Container
{
    private List<Slot> slots = new List<Slot>();
    private GameObject spawnedContainerPrefab;
    private Inventory containerInventory;
    private Inventory playerInventory;

    public Container(Inventory containerInventory, Inventory playerInventory)
    {
        this.containerInventory = containerInventory;
        this.playerInventory = playerInventory;
        OpenContainer();
    }

    /// <summary>
    ///     Draws a new object slot to the current container.
    /// </summary>
    /// <param name="inventory"> Where the slot will be added. </param>
    /// <param name="slotID"> Assigned ID for the new slot. </param>
    /// <param name="x"> X position where the slot will be instantiated. </param>
    /// <param name="y"> Y position where the slot will be instantiated. </param>
    /// <param name="slotSize"> New slot's scale. </param>
    public void AddSlotToContainer(Inventory inventory, int slotID, float x, float y, int slotSize)
    {
        GameObject spawnedSlot = Object.Instantiate(InventoryManager.INSTANCE.slotPrefab);
        Slot slot = spawnedSlot.GetComponent<Slot>();
        RectTransform slotRT = slot.GetComponent<RectTransform>();
        slot.SetSlot(inventory, slotID, this);
        spawnedSlot.transform.SetParent(spawnedContainerPrefab.transform);
        spawnedSlot.transform.SetAsFirstSibling();
        slotRT.anchoredPosition = new Vector2(x, y);
        slotRT.sizeDelta = Vector2.one * slotSize;
        slotRT.localScale = new Vector2(1, 1);
        slots.Add(slot);
    }

    /// <summary>
    ///     Refreshes every single slot in the inventory.
    /// </summary>
    public void updateSlots()
    {
        foreach(Slot slot in slots)
        {
            slot.UpdateSlot();
        }
    }

    /// <summary>
    ///     Opens the inventory container.
    /// </summary>
    public void OpenContainer()
    {
        spawnedContainerPrefab = Object.Instantiate(GetContainerPrefab(), InventoryManager.INSTANCE.transform);
        spawnedContainerPrefab.transform.SetAsFirstSibling();
    }

    /// <summary>
    ///     Colapses the inventory container.
    /// </summary>
    public void CloseContainer()
    {
        Object.Destroy(spawnedContainerPrefab);
    }

    //Need to be overriden, can not be left blank or null.
    public virtual GameObject GetContainerPrefab()
    {
        return null;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Useless till chests development is ready.
    public GameObject GetSpawnedContainer()
    {
        return spawnedContainerPrefab;
    }

    public Inventory GetContainerInventory()
    {
        return containerInventory;
    }

    public Inventory GetPlayerInventory()
    {
        return playerInventory;
    }
}
