using UnityEngine;
using System.Collections;

// base class for all characters
public class Character {

    // position of the Character
    public Vector3 position;
    
    // Health of the Character
    public int health;

    public Character(Tile tile, int _health) {
        position = new Vector3(0, 0, 0);

        this.position.x = tile.x;
        this.position.z = tile.z;

        health = _health;
    }

    protected void Move(Vector3 posChange) {
        position += posChange;
    }
    

}
