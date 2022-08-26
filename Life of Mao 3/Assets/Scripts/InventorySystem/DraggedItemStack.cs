using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
///     This class is used to create an auxiliary stack for easier inventory's management.
/// </summary>
public class DraggedItemStack : MonoBehaviour
{
    public Image itemIcon;
    public Text itemAmount;

    private ItemStack myStack = ItemStack.Empty;

    /// <summary>
    ///     Set's the wished stack as the dragged stack.
    /// </summary>
    public void SetDraggedStack(ItemStack stackIn)
    {
        myStack = stackIn;
    }

    /// <summary>
    ///     If the dragged stack is not empty, this function will draw it's icon and current count.
    /// </summary>
    private void DrawStack()
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
            DisableDragStack();
        }
    }

    /// <summary>
    ///     This function disables the dragged stack while it's empty.
    /// </summary>
    private void DisableDragStack()
    {
        itemIcon.enabled = false;
        itemAmount.text = string.Empty;
    }

    private void Update()
    {
        DrawStack();
        //Will detect the user's mouse position, so we can draw the dragged stack close to the cursor, by fixing the stack's position in 30u.
        Vector2 position = Mouse.current.position.ReadValue();
        position.x -= 30;
        position.y += 30;
        transform.position = position;
    }
}
