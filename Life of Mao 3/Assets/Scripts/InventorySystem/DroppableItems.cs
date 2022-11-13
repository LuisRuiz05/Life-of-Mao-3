using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableItems : MonoBehaviour
{
    public Item item;
    public int quantity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().myInventory.AddItem(new ItemStack(item, quantity));

            if (item.name == "PistolAmmo")
                other.GetComponent<PlayerController>().pistolAmmo += quantity;
            if (item.name == "UziAmmo")
                other.GetComponent<PlayerController>().uziAmmo += quantity;
            if (item.name == "RifleAmmo")
                other.GetComponent<PlayerController>().rifleAmmo += quantity;

            InventoryManager.INSTANCE.currentOpenContainer.updateSlots();

            Destroy(gameObject);
        }
    }
}
