using UnityEngine;
using System.Collections.Generic;

// A rectangle room

    //     innerTiles
    // /// edgeTiles

    ///////////////rightUpperTile
    ///         ///
    ///         ///
    ///         ///
    ///         ///
    ///////////////
//  leftunderTile

public class Room {

    Tile lowerTile;
    Tile upperTile;

    // Tiles, that are IN the room, but at the edge of the room
    List<Tile> edgeTiles;
    // Tiles, that are IN the room, and not at the edge of the room
    List<Tile> innerTiles;

    public Room(Tile leftUnderTile, Tile rightUpperTile) {

        lowerTile = leftUnderTile;
        upperTile = rightUpperTile;

        edgeTiles = new List<Tile>();
        innerTiles = new List<Tile>();

        AddTilesToLists();
    }

    void AddTilesToLists() {

        for (int i = 0; i < 1; i++) {
            for (int j = 0; j < 1; j++) {
                if (true) {

                }
            }
        }
    }
}
