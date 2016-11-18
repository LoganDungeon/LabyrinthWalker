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
    
    // lowerTile is the left under Tile of the room
    public Tile lowerTile {
        get;
        protected set;
    }

    // upperTile is the right upper Tile of the room
    public Tile upperTile {
        get;
        protected set;
    }

    // reference to the world
    private World world {
        get {
            return WorldController.Instance.world;
        }
    }

    // length of the room in X direction (walls not included)
    public int xLength {
        get {
            return upperTile.x - lowerTile.x + 1;
        }
    }

    // length of the room in Z direction (walls not included)
    public int zLength {
        get {
            return upperTile.z - lowerTile.z + 1;
        }
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
        // change the tiles to be wall free
        CreateRoom();
        
        // TODO
        // For now i want to add between 1 and 4 entrances, Later on i maybe want to create entrances, based on the size of the room
        AddEntrances(Random.Range(1,5));
    }

    // adds the Tiles in the room to the specific List, if the tiles are either edge tiles or not
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

    // goes through every Tile in the room , mark it as visited and removes the wall on it
    void CreateRoom() {
        for (int x = lowerTile.x-1; x <= upperTile.x+1; x++) {
            for (int z = lowerTile.z-1; z <= upperTile.z+1; z++) {
                Tile t = world.GetTileAt(x, z);
                if ( x==lowerTile.x-1 || x==upperTile.x+1 || z==lowerTile.z-1 || z==upperTile.z+1 ) {
                    // This are the walls of the room
                    t.isRoomWall = true;
                }
                else{
                    t.wall = false;
                }
            }
        }
    }

    // adds a certain number of entrances to the room by deleting a section of the wall that leads into the room
    void AddEntrances(int entrancesNumber) {
        List<Tile> possibleEntranceTiles = new List<Tile>();
        // create a list with all the possible entrance tiles
        foreach (Tile tile in edgeTiles) {
            if ((tile.x % 2 != 0) && (tile.z % 2 != 0) && tile.x != 1 && tile.x != world.Width-2 && tile.z != 1 && tile.z != world.Width-2) {
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
            // 8 possibilities:
            //  - 4 corners
            //  - 4 edges
            
            // 1 corner, left  lower
            if (possibleEntranceTiles[temp].x==lowerTile.x && possibleEntranceTiles[temp].z == lowerTile.z) {
                // we are on a corner, so there are 2 possible entrances
                int rand = Random.Range(0, 2);
                if (rand == 0) {
                    world.GetTileAt(possibleEntranceTiles[temp].x - 1, possibleEntranceTiles[temp].z).isRoomWall = false;
                    world.GetTileAt(possibleEntranceTiles[temp].x - 1, possibleEntranceTiles[temp].z).wall = false;
                }
                else if (rand == 1) {
                    world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z - 1).isRoomWall = false;
                    world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z - 1).wall = false;
                }
                continue;
            }
            // 2 corner, left  upper
            if (possibleEntranceTiles[temp].x == lowerTile.x && possibleEntranceTiles[temp].z == upperTile.z) {
                // we are on a corner, so there are 2 possible entrances
                int rand = Random.Range(0, 2);
                if (rand == 0) {
                    world.GetTileAt(possibleEntranceTiles[temp].x - 1, possibleEntranceTiles[temp].z).isRoomWall = false;
                    world.GetTileAt(possibleEntranceTiles[temp].x - 1, possibleEntranceTiles[temp].z).wall = false;
                }
                else if (rand == 1) {
                    world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z + 1).isRoomWall = false;
                    world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z + 1).wall = false;
                }
                continue;
            }
            // 3 corner, right lower
            if (possibleEntranceTiles[temp].x == upperTile.x && possibleEntranceTiles[temp].z == lowerTile.z) {
                // we are on a corner, so there are 2 possible entrances
                int rand = Random.Range(0, 2);
                if (rand == 0) {
                    world.GetTileAt(possibleEntranceTiles[temp].x + 1, possibleEntranceTiles[temp].z).isRoomWall = false;
                    world.GetTileAt(possibleEntranceTiles[temp].x + 1, possibleEntranceTiles[temp].z).wall = false;
                }
                else if (rand == 1) {
                    world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z - 1).isRoomWall = false;
                    world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z - 1).wall = false;
                }
                continue;
            }
            // 4 corner, right upper
            if (possibleEntranceTiles[temp].x == upperTile.x && possibleEntranceTiles[temp].z == upperTile.z) {
                // we are on a corner, so there are 2 possible entrances
                int rand = Random.Range(0, 2);
                if (rand == 0) {
                    world.GetTileAt(possibleEntranceTiles[temp].x + 1, possibleEntranceTiles[temp].z).isRoomWall = false;
                    world.GetTileAt(possibleEntranceTiles[temp].x + 1, possibleEntranceTiles[temp].z).wall = false;
                }
                else if (rand == 1) {
                    world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z + 1).isRoomWall = false;
                    world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z + 1).wall = false;
                }
                continue;
            }
            // 5 edge, left
            if (possibleEntranceTiles[temp].x == lowerTile.x) {
                // we are on an edge, so there is 1 possible entrances
                world.GetTileAt(possibleEntranceTiles[temp].x - 1, possibleEntranceTiles[temp].z).isRoomWall = false;
                world.GetTileAt(possibleEntranceTiles[temp].x - 1, possibleEntranceTiles[temp].z).wall = false;
                continue;
            }
            // 6 edge, upper
            if (possibleEntranceTiles[temp].z == upperTile.z) {
                world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z + 1).isRoomWall = false;
                world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z + 1).wall = false;
                continue;
            }
            // 7 edge, right
            if (possibleEntranceTiles[temp].x == upperTile.x) {
                // we are on a corner, so there is 1 possible entrances
                world.GetTileAt(possibleEntranceTiles[temp].x + 1, possibleEntranceTiles[temp].z).isRoomWall = false;
                world.GetTileAt(possibleEntranceTiles[temp].x + 1, possibleEntranceTiles[temp].z).wall = false;
                continue;
            }
            // 8 edge, lower
            if (possibleEntranceTiles[temp].z == lowerTile.z) {
                // we are on a corner, so there are 2 possible entrances
                world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z - 1).isRoomWall = false;
                world.GetTileAt(possibleEntranceTiles[temp].x, possibleEntranceTiles[temp].z - 1).wall = false;
                continue;
            }
            possibleEntranceTiles.RemoveAt(temp);
        }
    }
}
