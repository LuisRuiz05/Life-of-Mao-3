using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableItems : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().myInventory.AddItem(new ItemStack(item, 1));

            if (item.name == "PistolAmmo")
                other.GetComponent<PlayerController>().pistolAmmo++;
            if (item.name == "UziAmmo")
                other.GetComponent<PlayerController>().uziAmmo++;
            if (item.name == "RifleAmmo")
                other.GetComponent<PlayerController>().rifleAmmo++;

            InventoryManager.INSTANCE.currentOpenContainer.updateSlots();

            Destroy(gameObject);
        }
    }
}
