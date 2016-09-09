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

    public Tile lowerTile;
    public Tile upperTile;

    // Tiles, that are IN the room, but at the edge of the room
    public List<Tile> edgeTiles;
    // Tiles, that are IN the room, and not at the edge of the room
    public List<Tile> innerTiles;

    public Room(Tile leftUnderTile, Tile rightUpperTile) {

        lowerTile = leftUnderTile;
        upperTile = rightUpperTile;

        edgeTiles = new List<Tile>();
        innerTiles = new List<Tile>();

        AddTilesToLists();
    }

    void AddTilesToLists() {

        for (int x = lowerTile.x; x < upperTile.x + 1; x++) {
            for (int z = lowerTile.z; z < upperTile.z + 1; z++) {
                if (x == lowerTile.x || x == upperTile.x || z == lowerTile.z || z == upperTile.z) {
                    edgeTiles.Add(WorldController.Instance.world.GetTileAt(x, z));
                }else {
                    innerTiles.Add(WorldController.Instance.world.GetTileAt(x, z));
                }
            }
        }
    }
}
