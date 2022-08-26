using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This class is in charge of opening/closing the inventory and as a tool for managing the dragged stack.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager INSTANCE;

    private void Awake()
    {
        INSTANCE = this;
    }
    #endregion

    public GameObject slotPrefab;
    public List<ContainerGetter> containers = new List<ContainerGetter>();
    private Container currentOpenContainer;
    private ItemStack curDraggedStack = ItemStack.Empty;
    private GameObject spawnedDragStack;
    private DraggedItemStack dragStack;
    private Tooltip tooltip;

    private void Start()
    {
        dragStack = GetComponentInChildren<DraggedItemStack>();
        tooltip = GetComponentInChildren<Tooltip>();
    }

    /// <summary>
    ///     Returns the opened inventory prefab if there's one.
    /// </summary>
    public GameObject GetContainerPrefab(string name)
    {
        foreach(ContainerGetter container in containers)
        {
            if(container.containerName == name)
            {
                return container.containerPrefab;
            }
        }
        return null;
    }

    /// <summary>
    ///     Opens the inventory.
    /// </summary>
    /// <param name="container"> Receives a container where the inventory will be initialized. </param>
    public void OpenContainer(Container container)
    {
        if(currentOpenContainer != null)
        {
            currentOpenContainer.CloseContainer();
        }
        currentOpenContainer = container;
    }

    /// <summary>
    ///     Closes the inventory.
    /// </summary>
    public void CloseContainer()
    {
        if(currentOpenContainer != null)
        {
            currentOpenContainer.CloseContainer();
        }
    }

    /// <summary>
    ///     Returns the current dragged stack if there's one, if not, you'll get an empty one.
    /// </summary>
    public ItemStack GetDraggedItemStack()
    {
        return curDraggedStack;
    }

    /// <summary>
    ///     Gives a stack value to the current dragged stack.
    /// </summary>
    /// <param name="stackIn"> Stack which will be assigned to the dragged stack. </param>
    public void SetDraggedItemStack(ItemStack stackIn)
    {
        dragStack.SetDraggedStack(curDraggedStack = stackIn);
    }

    /// <summary>
    ///     Draws a tool tip of the hovered item, near the cursor's position.
    /// </summary>
    /// <param name="itemName"> Displayed name in the tooltip. </param>
    public void DrawTooltip(string itemName)
    {
        tooltip.SetToolTip(itemName);
    }
}

/// <summary>
///     This class looks for a container at the Inventory's UI panel.
/// </summary>
[System.Serializable]
public class ContainerGetter
{
    public string containerName;
    public GameObject containerPrefab;
}