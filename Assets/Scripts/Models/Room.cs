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

    World world {
        get {
            return WorldController.Instance.world;
        }
    }

    public int XLength {
        get {
            return upperTile.x - lowerTile.x + 1;
        }
        protected set { }
    }

    public int ZLength {
        get {
            return upperTile.z - lowerTile.z + 1;
        }
        protected set { }
    }

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

        // For now i want to add between 1 and 4 entrances, Later on i maybe want to create entrances, based on the size of the room
        AddEntrances(Random.Range(1,5));
    }

    void AddTilesToLists() {

        for (int x = lowerTile.x; x <= upperTile.x; x++) {
            for (int z = lowerTile.z; z <= upperTile.z; z++) {
                if (x == lowerTile.x || x == upperTile.x || z == lowerTile.z || z == upperTile.z) {
                    edgeTiles.Add(world.GetTileAt(x, z));
                }else {
                    innerTiles.Add(world.GetTileAt(x, z));
                }
            }
        }
    }

    void ClearRoom() {

        for (int x = lowerTile.x; x <= upperTile.x; x++) {
            for (int z = lowerTile.z; z <= upperTile.z; z++) {
                Tile t = world.GetTileAt(x, z);
                t.wall = false;
                t.visited = true;
                t.isInRoom = true;
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
            // check if there are possible entrance Tiles left over
            if (possibleEntranceTiles.Count == 0) {
                return;
            }
            // chose a random tile for the entrance and create the entrance, aka mark it as "not in the room", so the maze Generator can go out of the room again.
            int temp = Random.Range(0, possibleEntranceTiles.Count);
            possibleEntranceTiles[temp].isInRoom = false;
            possibleEntranceTiles.RemoveAt(temp);
        }
    }
}
