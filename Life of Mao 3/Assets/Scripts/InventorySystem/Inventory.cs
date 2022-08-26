using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This classes manages the player's inventory content.
/// </summary>
public class Inventory
{
    private List<ItemStack> inventoryContents = new List<ItemStack>();

    public Inventory(int size)
    {
        for(int i = 0; i < size; i++)
        {
            inventoryContents.Add(new ItemStack(i));
        }
    }

    /// <summary>
    ///     Adds an item to the player's inventory content.
    /// </summary>
    /// <param name="input"> Indicates the item that is willing to be added. </param>
    /// <returns> Returns a boolean which will indicate if it was added successfully. </returns>
    public bool AddItem(ItemStack input)
    {
        foreach (ItemStack stack in inventoryContents)
        {
            // If itemstack is empty, we can add the item succesfully.
            if(stack.IsEmpty()){
                stack.SetStack(input);
                return true;
            }
            // Current itemstack do has an item.
            else
            {
                // The item provided by the user and the one in the item stack are of the same kind.
                if(ItemStack.AreItemsEqual(input, stack))
                {
                    // If the current stack item has enough space to store all the new items, we can add them succesfully.
                    if (stack.canAddToo(input.GetCount()))
                    {
                        stack.IncreaseAmount(input.GetCount());
                        return true;
                    }
                    // If the current stack item has enough space to store some of the new items, we can add them succesfully and we keep the difference.
                    else
                    {
                        int difference = (stack.GetCount() + input.GetCount()) - stack.GetItem().maxStackSize;
                        stack.SetCount(stack.GetItem().maxStackSize);
                        input.SetCount(difference);
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    ///     Returns the stack in the selected slot.
    /// </summary>
    /// <param name="index"> Receives the index of the inventory's slot where the program will search. </param>
    /// <returns> The stack saved in the asked position. </returns>
    public ItemStack GetStackInSlot(int index)
    {
        return inventoryContents[index];
    }

    /// <summary>
    ///     Returns the content of the whole inventory.
    /// </summary>
    public List<ItemStack> GetInventoryStacks()
    {
        return inventoryContents;
    }
}
