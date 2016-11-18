using UnityEngine;
using System.Collections;

// an Inventory is used in something like a Chest, a Player, or an NPC
public class Inventory {

    // and Inventory Slot is a Space where a single item Type can be stored. 
    // The Slotsize will determine, how much of a type can be stored in there
    // TODO: implement SlotZise and maybe create an Object for a Slot
    public int inventorySlots;

    // creates an Inventory with a certain number of slots
    public Inventory(int _inventorySlots) {
        inventorySlots = _inventorySlots;
    }
}
