using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class is in charge of drawing the hotbar in the UI.
/// </summary>
public class ContainerPlayerHotbar : Container
{
    /// <summary>
    ///     This class draws a 6 slot hotbar.
    /// </summary>
    public ContainerPlayerHotbar(Inventory containerInventory, Inventory playerInventory) : base(containerInventory, playerInventory)
    {
        for (int i = 0; i < 6; i++)
        {
            AddSlotToContainer(playerInventory, i, 20 + (90 * i), 0, 100);
        }
    }

    /// <summary>
    ///     Returns the opened inventory hotbar if there's one.
    /// </summary>
    public override GameObject GetContainerPrefab()
    {
        return InventoryManager.INSTANCE.GetContainerPrefab("Player Hotbar");
    }
}
