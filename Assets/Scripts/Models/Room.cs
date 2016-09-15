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

        // change the tiles to be wall fre
        ClearRoom();

        AddEntrances(1);
    }

    void AddTilesToLists() {

        for (int x = lowerTile.x; x <= upperTile.x; x++) {
            for (int z = lowerTile.z; z <= upperTile.z; z++) {
                if (x == lowerTile.x || x == upperTile.x || z == lowerTile.z || z == upperTile.z) {
                    edgeTiles.Add(WorldController.Instance.world.GetTileAt(x, z));
                }else {
                    innerTiles.Add(WorldController.Instance.world.GetTileAt(x, z));
                }
            }
        }
    }

    void ClearRoom() {

        for (int x = lowerTile.x; x <= upperTile.x; x++) {
            for (int z = lowerTile.z; z <= upperTile.z; z++) {
                WorldController.Instance.world.GetTileAt(x, z).wall = false;
                WorldController.Instance.world.GetTileAt(x, z).visited = true;
            }
        }
    }

    void AddEntrances(int entrancesNumber) {

        List<Tile> possibleEntranceTiles = new List<Tile>();

        foreach (Tile tile in edgeTiles) {
            if ((tile.x % 2 != 0) && (tile.z % 2 != 0)) {
                possibleEntranceTiles.Add(tile);
            }
        }

        for (int i = 0; i < entrancesNumber; i++) {
            // chose a random tile for the entrance and create the entrance, aka delete the wall
            int temp = Random.Range(0, possibleEntranceTiles.Count);
            possibleEntranceTiles[temp].visited = false;
            possibleEntranceTiles.RemoveAt(temp);
        }
    }
}
