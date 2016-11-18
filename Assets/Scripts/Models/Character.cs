using UnityEngine;
using System.Collections;

// base class for all characters
public class Character {

    // position of the Character
    public Vector3 position;
    
    // Health of the Character
    public int health;

    // Contructor will likely never be called on its own, only gets called by its inheriting classes
    public Character(Tile tile, int _health) {
        // set the position and the health
        position = new Vector3(tile.x, 0, tile.z);
        health = _health;
    }

    // simple function to move the Character.
    // TODO: Maybe move to the EnemyCharacter Class, because the Player will never need this function.
    //       PLayerMovement is handled with the FPSWakerEnhanced Class
    protected void Move(Vector3 posChange) {
        position += posChange;
    }
    

}
