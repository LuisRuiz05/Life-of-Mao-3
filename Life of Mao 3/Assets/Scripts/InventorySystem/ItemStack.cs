using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class works is used as aun auxiliary tool for stacking items, and some few more options.
/// </summary>
public class ItemStack
{
    public static ItemStack Empty = new ItemStack();
    public Item item;
    public int count;
    public int slotID;

    // Constructors
    public ItemStack()
    {
        this.item = null;
        this.count = 0;
        this.slotID = -1;
    }

    public ItemStack(int slotID)
    {
        this.item = null;
        this.count = 0;
        this.slotID = slotID;
    }

    public ItemStack(Item item, int count)
    {
        this.item = item;
        this.count = count;
        this.slotID = -1;
    }

    public ItemStack(Item item, int count, int slotID)
    {
        this.item = item;
        this.count = count;
        this.slotID = slotID;
    }

    // Getters and Setters
    public Item GetItem()
    {
        return this.item;
    }

    public int GetCount()
    {
        return this.count;
    }

    /// <summary>
    ///     Sets the item's type and its quantity.
    /// </summary>
    public void SetStack(ItemStack stackIn)
    {
        this.item = stackIn.GetItem();
        this.count = stackIn.GetCount();
    }

    /// <summary>
    ///     Checks if the current stack is empty or not.
    /// </summary>
    /// <returns> A boolean with the operations result. </returns>
    public bool IsEmpty()
    {
        return this.count < 1;
    }

    /// <summary>
    ///     Increase the stack's item quantity.
    /// </summary>
    /// <param name="amount"> Indicates the quantity that will be added to the count. </param>
    public void IncreaseAmount(int amount)
    {
        this.count += amount;
    }

    /// <summary>
    ///     Decrease the stack's item quantity.
    /// </summary>
    /// <param name="amount"> Indicates the quantity that will be substracted to the count. </param>
    public void DecreaseAmount(int amount)
    {
        this.count -= amount;
        if(count == 0)
        {
            this.item = null;
        }
    }

    /// <summary>
    ///     Sets an specific amount in the indicated item stack.
    /// </summary>
    /// <param name="amount"> Indicates the quantity that will be set up. </param>
    public void SetCount(int amount)
    {
        this.count = amount;
    }

    /// <summary>
    ///     Checks if the current stack can hold some more items.
    /// </summary>
    /// <param name="amount"> Quantity of the wished items to add. </param>
    /// <returns> A boolean with the operations result. </returns>
    public bool canAddToo(int amount)
    {
        return (this.count + amount) <= this.item.maxStackSize;
    }

    /// <summary>
    ///     Splits the current stack.
    /// </summary>
    /// <param name="amount"> Indicates an arbitrary number indicated by the user as it's wished to split the stack based on this quantity. </param>
    /// <returns> A new stack of the same item, with the minimum quantity between the provided quantity and it's count. </returns>
    public ItemStack SplitStack(int amount)
    {
        int i = Mathf.Min(amount, count);
        ItemStack copiedStack = this.Copy();
        copiedStack.SetCount(i);
        this.DecreaseAmount(i);
        return copiedStack;
    }

    /// <summary>
    ///     Clones the actual item stack.
    /// </summary>
    /// <returns> A clone of the actual item stack. </returns>
    public ItemStack Copy()
    {
        return new ItemStack(this.item, this.count, this.slotID);
    }

    /// <summary>
    ///     Compares if the actual item is equal to another.
    /// </summary>
    /// <param name="stackIn"> Item to be compared. </param>
    /// <returns> A boolean with the operations result. </returns>
    public bool IsItemEqual(ItemStack stackIn)
    {
        return !stackIn.IsEmpty() && this.item == stackIn.GetItem();
    }

    /// <summary>
    ///     Compares if two provided items are equal.
    /// </summary>
    /// <param name="stackA"> First item to be compared </param>
    /// <param name="stackB"> Second item to be compared </param>
    /// <returns> A boolean with the operations result. </returns>
    public static bool AreItemsEqual(ItemStack stackA, ItemStack stackB)
    {
        return stackA == stackB ? true : (!stackA.IsEmpty() && !stackB.IsEmpty() ? stackA.IsItemEqual(stackB) : false);
    }
}
