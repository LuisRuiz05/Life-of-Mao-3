using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class is in charge of drawing the inventory in the UI.
/// </summary>
public class ContainerPlayerInventory : Container
{
    /// <summary>
    ///     This class draws a 24 slot inventory.
    /// </summary>
    public ContainerPlayerInventory(Inventory containerInventory, Inventory playerInventory) : base (containerInventory, playerInventory)
    {
        for(int i=0; i<6; i++)
        {
            AddSlotToContainer(playerInventory, 6 + i, 20 + (90 * i), -15, 100);
        }
        for (int i = 0; i < 6; i++)
        {
            AddSlotToContainer(playerInventory, 12 + i, 20 + (90 * i), -100, 100);
        }
        for (int i = 0; i < 6; i++)
        {
            AddSlotToContainer(playerInventory, 18 + i, 20 + (90 * i), -185, 100);
        }
        for (int i = 0; i < 6; i++) //Hotbar
        {
            AddSlotToContainer(playerInventory, i, 20 + (90 * i), -295, 100);
        }
    }

    /// <summary>
    ///     Returns the opened inventory prefab if there's one.
    /// </summary>
    public override GameObject GetContainerPrefab()
    {
        return InventoryManager.INSTANCE.GetContainerPrefab("Player Inventory");
    }
}
