using UnityEngine;
using System.Collections;

// every non Player Character in the game
public class EnemyCharacter : Character
{

    // Creates an NPC at the tiles position
    // TODO: will likely need more like and Inventory and such stuff
    public EnemyCharacter(Tile tile, int health) : base(tile, health)
    {

    }
}
