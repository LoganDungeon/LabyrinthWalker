using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

    // Inventory of the Player
    private Inventory _playerInventory;

    // saturation will go down with the time and stamina will be used to sprint
    public int Saturation {
        get;
        protected set;
    }

    public int Stamina {
        get;
        protected set;
    }

    // visuals of the Character
    public PlayerCharacterGOController PcgoC;

    // creates the Character for the Player. tile - position to spawn at, health, stamina, 
    // saturation - stats of the player, inventorySlots - Number of Spaces in the Inventory
    public PlayerCharacter(Tile tile, int health, int saturation, int stamina, int inventorySlots): base(tile, health) {
        PcgoC = Object.FindObjectOfType<PlayerCharacterGOController>();
        PcgoC.CreateCharacterGameObject(tile);

        this.Saturation = saturation;
        this.Stamina = stamina;

        _playerInventory = new Inventory(inventorySlots);
    }

    public int GetInventorySpaces() {
        return _playerInventory.Slots.Length;
    }
}
