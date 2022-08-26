using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///     Acts as a secondary container which will hold individual type items.
/// </summary>
public class Slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler ,IPointerExitHandler
{
    public Image itemIcon;
    public Text itemAmount;
    private int slotID;
    private ItemStack myStack;
    private Container attachedContainer;
    private InventoryManager inventoryManager;

    /// <summary>
    ///     Set's an specific slot on an inventory container.
    /// </summary>
    public void SetSlot(Inventory attachedInventory, int slotID, Container attachedContainer)
    {
        this.slotID = slotID;
        this.attachedContainer = attachedContainer;
        myStack = attachedInventory.GetStackInSlot(slotID);
        inventoryManager = InventoryManager.INSTANCE;
        UpdateSlot();
    }

    /// <summary>
    ///     Refreshes the slot's information visually.
    /// </summary>
    public void UpdateSlot()
    {
        if (!myStack.IsEmpty())
        {
            itemIcon.enabled = true;
            itemIcon.sprite = myStack.GetItem().itemIcon;

            if (myStack.GetCount() > 1)
            {
                itemAmount.text = myStack.GetCount().ToString();
            }
            else
            {
                itemAmount.text = string.Empty;
            }
        }
        else
        {
            itemIcon.enabled = false;
            itemAmount.text = string.Empty;
        }
    }

    /// <summary>
    ///     Refreshes the slot's information.
    /// </summary>
    /// <param name="stackIn"> Receives the information of the wished dragged or copied stack. </param>
    private void SetSlotContents(ItemStack stackIn)
    {
        myStack.SetStack(stackIn);
        UpdateSlot();
    }

    /// <summary>
    ///     Receives mouse's input data.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        ItemStack curDraggedStack = inventoryManager.GetDraggedItemStack();
        ItemStack stackCopy = myStack.Copy();

        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(curDraggedStack, stackCopy);
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick(curDraggedStack, stackCopy);
        }
    }

    private void SetTooltip(string nameIn)
    {
        inventoryManager.DrawTooltip(nameIn);
    }

    /// <summary>
    ///     Called on a left click on a slot.
    /// </summary>
    private void OnLeftClick(ItemStack curDraggedStack, ItemStack stackCopy)
    {
        // If we've got an empty dragged stack and some item in the slot, we'll take the items and hold them in the dragged stack.
        if (!myStack.IsEmpty() && curDraggedStack.IsEmpty())
        {
            inventoryManager.SetDraggedItemStack(stackCopy);
            this.SetSlotContents(ItemStack.Empty);
            SetTooltip(string.Empty);
        }
        // If we've got a dragged stack and an the clicked slot is empty, we'll drop the dragged stack on the pressed slot.
        if (myStack.IsEmpty() && !curDraggedStack.IsEmpty())
        {
            this.SetSlotContents(curDraggedStack);
            inventoryManager.SetDraggedItemStack(ItemStack.Empty);
            SetTooltip(myStack.GetItem().itemName);
        }
        // We've got a dragged stack and we've clicked a slot that it's not empty.
        if (!myStack.IsEmpty() && !curDraggedStack.IsEmpty())
        {
            // Dragged stack item and clicked slot items are the same.
            if(ItemStack.AreItemsEqual(stackCopy, curDraggedStack))
            {
                // If there's enough space to add them all, will add the items to the clicked slot.
                if (stackCopy.canAddToo(curDraggedStack.GetCount()))
                {
                    stackCopy.IncreaseAmount(curDraggedStack.GetCount());
                    this.SetSlotContents(stackCopy);
                    inventoryManager.SetDraggedItemStack(ItemStack.Empty);
                    SetTooltip(myStack.GetItem().itemName);
                }
                // If there's enough space to add some of them, will add items to the clicked slot till we run out of storage and will keep the difference at the dragged stack.
                else
                {
                    int difference = (stackCopy.GetCount() + curDraggedStack.GetCount()) - stackCopy.GetItem().maxStackSize;
                    stackCopy.SetCount(myStack.GetItem().maxStackSize);
                    ItemStack dragCopy = curDraggedStack.Copy();
                    dragCopy.SetCount(difference);
                    this.SetSlotContents(stackCopy);
                    inventoryManager.SetDraggedItemStack(dragCopy);
                    SetTooltip(string.Empty);
                }
            }
            // Dragged stack item and clicked slot items are different, so will change places.
            else
            {
                ItemStack curDragCopy = curDraggedStack.Copy();
                this.SetSlotContents(curDragCopy);
                inventoryManager.SetDraggedItemStack(stackCopy);
                SetTooltip(string.Empty);
            }
        }
    }

    /// <summary>
    ///     Called on a left click on a slot.
    /// </summary>
    private void OnRightClick(ItemStack curDraggedStack, ItemStack stackCopy)
    {
        // If we've got an empty dragged stack and some item in the clicked slot, we'll try to get half of the items contained in the clicked slot and get them to the dragged stack.
        if (!myStack.IsEmpty() && curDraggedStack.IsEmpty())
        {
            ItemStack stack = stackCopy.SplitStack(stackCopy.GetCount() / 2);
            inventoryManager.SetDraggedItemStack(stack);
            this.SetSlotContents(stackCopy);
            SetTooltip(string.Empty);
        }
        // If we've got a dragged stack and an the clicked slot is empty, we'll add the clicked slot just one of the dragged stack's items.
        if (myStack.IsEmpty() && !curDraggedStack.IsEmpty())
        {
            this.SetSlotContents(new ItemStack(curDraggedStack.GetItem(), 1));
            ItemStack curDragCopy = curDraggedStack.Copy();
            curDragCopy.DecreaseAmount(1);
            inventoryManager.SetDraggedItemStack(curDragCopy);
            SetTooltip(string.Empty);
        }
        // If we've got a dragged stack and an the clicked slot is not empty, we'll add the clicked slot one of the dragged stack's items if they contain the same item and it has enough space.
        if (!myStack.IsEmpty() && !curDraggedStack.IsEmpty())
        {
            if (ItemStack.AreItemsEqual(stackCopy, curDraggedStack))
            {
                if (myStack.canAddToo(1))
                {
                    stackCopy.IncreaseAmount(1);
                    this.SetSlotContents(stackCopy);
                    ItemStack dragCopy = curDraggedStack.Copy();
                    dragCopy.DecreaseAmount(1);
                    inventoryManager.SetDraggedItemStack(dragCopy);
                    SetTooltip(string.Empty);
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemStack curDraggedStack = inventoryManager.GetDraggedItemStack();

        if(!myStack.IsEmpty() && curDraggedStack.IsEmpty())
        {
            SetTooltip(myStack.GetItem().itemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetTooltip(string.Empty);
    }
}
