using UnityEngine;
using System.Collections;

// an Inventory is used in something like a Chest, a Player, or an NPC
public class Inventory {
    // and Inventory Slot is a Space where a single item Type can be stored. 
    // The Slotsize will determine, how much of a type can be stored in there
    // TODO: implement SlotZise

    // TODO: empty slots can be either null or a specific "empty Item".
    // guess null will be the way to go.
    public Item[] Slots {
        get;
        private set;
    }

    // creates an empty Inventory with a certain number of slots
    public Inventory(int inventorySlots) {
        this.Slots = new Item[inventorySlots];
    }

    public void AddItem(Item item) {
        // for now i have 2 different "kinds" of items: 
        // equipables are not stackable and have lifepoints etc.
        // consumables are stackable (maybe i have to change the name later)
        if (item is Consumable) {
            for (int i = 0; i < this.Slots.Length; i++) {
                if (this.Slots[i] == null || this.Slots[i].Id != item.Id) continue;
                // found the inventoryslot which contains the item
                if(((Consumable)this.Slots[i]).Quantity + ((Consumable)item).Quantity <= ((Consumable)item).MaxQuantity) {
                    ((Consumable)this.Slots[i]).Quantity += ((Consumable) item).Quantity;
                }
            }
        }


        foreach (Item itemInList in this.Slots) {
        }
        //if (slots[inventorySlot] == null) {
        //    slots[inventorySlot] = item;
        //}
        //else if (slots[inventorySlot].id == item.id) {
        //    if (slots[inventorySlot] is Consumable && item is Consumable) {
        //        if (((Consumable) slots[inventorySlot]).quantity + ((Consumable)item).quantity <= ((Consumable)item).maxQuantity) {

        //        }
        //    }
        //}
    }
}