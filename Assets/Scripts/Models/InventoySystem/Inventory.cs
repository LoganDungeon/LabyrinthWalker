using UnityEngine;
using System.Collections;

// an Inventory is used in something like a Chest, a Player, or an NPC
public class Inventory {
    // and Inventory Slot is a Space where a single item Type can be stored. 
    // The Slotsize will determine, how much of a type can be stored in there
    // TODO: implement SlotZise

    // TODO: empty slots can be either null or a specific "empty Item".
    // guess null will be the way to go.
    private Item[] slots;

    // creates an empty Inventory with a certain number of slots
    public Inventory(int inventorySlots) {
        this.slots = new Item[inventorySlots];
    }

    public void addItem(Item item) {
        // for now i have 2 different "kinds" of items: 
        // equipables are not stackable and have lifepoints etc.
        // consumables are stackable (maybe i have to change the name later)
        if (item is Consumable) {
            for (int i = 0; i < slots.Length; i++) {
                if (slots[i] == null)
                    continue;
                if (slots[i].id == item.id) {
                    // found the inventoryslot which contains the item
                    if (((Consumable)slots[i]).quantity + ((Consumable)item).quantity <= ((Consumable)item).maxQuantity) {
                        ((Consumable) slots[i]).quantity += ((Consumable) item).quantity;
                    }
                }
            }
        }


        foreach (Item itemInList in slots) {
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