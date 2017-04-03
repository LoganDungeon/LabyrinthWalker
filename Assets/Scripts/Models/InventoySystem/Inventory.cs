using UnityEngine;
using System.Collections;

// an Inventory is used in something like a Chest, a Player, or an NPC
public class Inventory
{
    // and Inventory Slot is a Space where a single item Type can be stored. 
    // The Slotsize will determine, how much of a type can be stored in there
    // TODO: implement SlotZise

    // TODO: empty slots can be either null or a specific "empty Item".
    // guess null will be the way to go.
    public Item[] Slots
    {
        get;
        private set;
    }

    // creates an empty Inventory with a certain number of slots
    public Inventory(int inventorySlots)
    {
        this.Slots = new Item[inventorySlots];
    }

    public Item AddItem(Item item)
    {
        // for now i have 2 different "kinds" of items: 
        // equipables are not stackable and have lifepoints etc.
        // consumables are stackable (maybe i have to change the name later)
        if(item is Consumable)
        {
            // item is stackable, so either the item is already in the inventory or an empty slot is needed.
            // first check if this item is already in the list
            for(int i = 0; i < this.Slots.Length; i++)
            {
                //if the slot is empty or contains an other item, just go on
                if(this.Slots[i] == null || this.Slots[i].Id != item.Id ||
                    (this.Slots[i].Id == item.Id && ((Consumable)this.Slots[i]).Quantity == ((Consumable)this.Slots[i]).MaxQuantity))
                    continue;
                // found the inventoryslot which contains the item
                // if the slot can hold all of the new items, just add them together and return
                if(((Consumable)this.Slots[i]).Quantity + ((Consumable)item).Quantity <= ((Consumable)item).MaxQuantity)
                {
                    ((Consumable)this.Slots[i]).Quantity += ((Consumable)item).Quantity;
                    // the whole itemStack has added to the inventory, so return an emtpy "remaining item"
                    return null;
                }
                else
                {
                    // the inventory is in the list, but it is not possible to add the whole stack
                    // so fill up the slot to the max and try to add the rest to other slots In the inventory
                    ((Consumable)item).Quantity = ((Consumable)item).Quantity + ((Consumable)this.Slots[i]).Quantity -
                                                   ((Consumable)item).MaxQuantity;
                    ((Consumable)this.Slots[i]).Quantity = ((Consumable)this.Slots[i]).MaxQuantity;
                    return AddItem(item);
                }
            }
            // at this point i know the item is not in the inventory and i need a new (empty) slot
            for(int i = 0; i < this.Slots.Length; i++)
            {
                if(this.Slots[i] != null)
                    continue;
                // empty slot found -> add the item and return
                this.Slots[i] = item;
                // no remaining items
                return null;
            }
            // the inventory is full and the item can not be deployed so return the item
            return item;
        }
        else if(item is Equipable)
        {
            // Need an empty inventory slot
            for(int i = 0; i < this.Slots.Length; i++)
            {
                if(this.Slots[i] != null)
                    continue;
                this.Slots[i] = item;
                return null;
            }
            // inventory is full
            return item;
        }
        // this shoud not be reached ever
        Debug.Log("Error: AddItem:" + item.Id);
        return null;
    }
}