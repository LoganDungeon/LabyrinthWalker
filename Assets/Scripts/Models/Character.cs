using UnityEngine;
using System.Collections;

// base class for all characters
public abstract class Character
{

    // position of the Character
    protected Vector3 Position
    {
        get;
        set;
    }

    // Health of the Character
    public int Health
    {
        get;
        set;
    }

    // Contructor will likely never be called on its own, only gets called by its inheriting classes
    protected Character(Tile tile, int health)
    {
        // set the position and the health
        this.Position = new Vector3(tile.X, 0, tile.Z);
        this.Health = health;
    }

    // simple function to move the Character.
    // TODO: Maybe move to the EnemyCharacter Class, because the Player will never need this function.
    //       PLayerMovement is handled with the FPSWakerEnhanced Class
    protected void Move(Vector3 posChange)
    {
        this.Position += posChange;
    }


}
