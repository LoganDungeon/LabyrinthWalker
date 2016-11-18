using UnityEngine;
using System.Collections;

// every non Player Character in the game
public class NPCharacter : Character {

    // Creates an NPC at the tiles position
    // TODO: will likely need more like and Inventory and such stuff
    public NPCharacter(Tile tile, int _health): base(tile, _health) {

    }
}
