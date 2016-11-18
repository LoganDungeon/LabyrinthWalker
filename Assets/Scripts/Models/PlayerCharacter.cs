using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

    // Inventory of the Player
    public Inventory playerInventory;

    // saturation will go down with the time and stamina will be used to sprint
    public int saturation;
    public int stamina;

    // visuals of the Character
    public PlayerCharacterGOController pcgoC;

    // creates the Character for the Player. tile - position to spawn at, health, stamina, 
    // saturation - stats of the player, inventorySlots - Number of Spaces in the Inventory
    public PlayerCharacter(Tile tile, int _health, int _saturation, int _stamina, int inventorySlots): base(tile, _health) {

        pcgoC = GameObject.FindObjectOfType<PlayerCharacterGOController>();
        pcgoC.CreateCharacterGameObject(tile);

        saturation = _saturation;
        stamina = _stamina;

        playerInventory = new Inventory(inventorySlots);
    }
}
