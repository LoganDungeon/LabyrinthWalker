using UnityEngine;
using System.Collections.Generic;

// static class which will contain all Generating functions
public static class MapGenerator {

    // reference to the world
    private static World world = WorldController.Instance.world;

    ////////////////////////////////////////////////////////////////////////////////////
    /// Algorithm comes from https://en.wikipedia.org/wiki/Maze_generation_algorithm ///
    /// Depth First Search Recursive Backtracker                                     ///
    ////////////////////////////////////////////////////////////////////////////////////
    public static void CreateMaze() {
        Tile currentTile;
        Tile nextTile;
        Stack<Tile> visitedTiles = new Stack<Tile>();
        int randX = Random.Range(1, world.Width - 1);
        int randZ = Random.Range(1, world.Height - 1);
        // first i will get a random tile to start from, which has not been "visited" yet (outside of a room)
        do {
            while (randX % 2 == 0) {
                randX = Random.Range(1, world.Width - 1);
            }
            while (randZ % 2 == 0) {
                randZ = Random.Range(1, world.Height - 1);
            }
        }
        while (world.GetTileAt(randX, randZ).visited);

        //===== FIRST STEP =====//
        // set the starting tile
        currentTile = world.GetTileAt(randX, randZ);
        // mark the startingTile as visited
        currentTile.visited = true;
        currentTile.wall = false;

        //===== SECOND STEP =====//
        // while there are tiles with visited == false
        while ( AreThereUnvisitedTiles() ) {
            //=== First Substep ===//
            // first get all the neighbours of the tile, that has not been visited yet
            List<Tile> neighbours = GetNeighbours(currentTile);
            // Check if the current Tile has any unvisited neighbours
            if (neighbours.Count > 0) {
                //=1=//
                // chose a random Neighbour
                nextTile = neighbours[Random.Range(1, 1000) % (neighbours.Count)];
                //=2=//
                // add the current Tile to the stack
                visitedTiles.Push(nextTile);
                //=3=//
                // remove the wall between the currentTile and the nextTile
                GetMiddleTile(currentTile, nextTile).wall = false;
                nextTile.wall = false;
                //=4=//
                // go to the nextTile and mark it as visited
                currentTile = nextTile;
                currentTile.visited = true;
            }
            // there are no unvisited neighbours and we go back
            else if( visitedTiles.Count > 0 ) {
                currentTile = visitedTiles.Pop();
            }
        }    
    }

    static bool AreThereUnvisitedTiles() {
        // go through all of the tiles and return true, if there is at least one unvisited tile
        for (int i = 1; i < world.Width; i+=2) {
            for (int j = 1; j < world.Height; j+=2) {
                if(world.GetTileAt(i, j).visited == false) {
                    return true;
                }
            }
        }
        return false;
    }

    // returns a List with all neighbours, that has not been visited yet
    static List<Tile> GetNeighbours( Tile tile) {
        List<Tile> neighbours = new List<Tile>();
        // add the north tile
        if ((tile.z < world.Height - 3 && world.GetTileAt(tile.x, tile.z + 2).visited == false) && GetMiddleTile(tile, world.GetTileAt(tile.x, tile.z + 2)).isRoomWall == false) {
            neighbours.Add(world.GetTileAt(tile.x, tile.z + 2));
        }
        // add the east tile
        if ((tile.x < world.Width - 3 && world.GetTileAt(tile.x + 2, tile.z).visited == false) && GetMiddleTile(tile, world.GetTileAt(tile.x + 2, tile.z)).isRoomWall == false) {
            neighbours.Add(world.GetTileAt(tile.x + 2, tile.z));
        }
        // add the south tile
        if ((tile.z >= 3 && world.GetTileAt(tile.x, tile.z - 2).visited == false) && GetMiddleTile(tile, world.GetTileAt(tile.x, tile.z - 2)).isRoomWall == false) {
            neighbours.Add(world.GetTileAt(tile.x, tile.z - 2));
        }
        // add the west tile
        if ((tile.x >= 3 && world.GetTileAt(tile.x - 2, tile.z).visited == false) && GetMiddleTile(tile, world.GetTileAt(tile.x - 2, tile.z)).isRoomWall == false) {
            neighbours.Add(world.GetTileAt(tile.x - 2, tile.z));
        }
        return neighbours;
    }

    // returns the tile inbetween of the given two tiles
    static Tile GetMiddleTile( Tile currentTile, Tile nextTile ) {
        return WorldController.Instance.world.GetTileAt( (currentTile.x + nextTile.x)/2, (currentTile.z + nextTile.z)/2 );
    }

    // creates the Rooms and returns a List with information bout them
    public static List<Room> CreateRooms() {

        List<Room> rooms = new List<Room>();

        // number of the atempts, how often the algorithm will try to place rooms
        int atempts = Mathf.FloorToInt(Mathf.Sqrt(WorldController.Instance.world.Height * WorldController.Instance.world.Width));

        // size of the rooms will be between 3 and a certain calculated value based on the world width/height
        int minRoomHeight = 3;
        int minRoomWidth= 3;
        int maxRoomHeight = (Mathf.FloorToInt(Mathf.Pow(Mathf.Pow(world.Height, 2f), 1 / 3f) - (Mathf.Sqrt((Mathf.Pow(Mathf.Pow(world.Height, 2f), 1 / 3f))))));
        int maxRoomWidth  = (Mathf.FloorToInt(Mathf.Pow(Mathf.Pow(world.Width, 2f), 1 / 3f) - (Mathf.Sqrt((Mathf.Pow(Mathf.Pow(world.Width, 2f), 1 / 3f))))));
        if (maxRoomHeight % 2 == 0)
            maxRoomHeight++;
        if (maxRoomWidth % 2 == 0)
            maxRoomWidth++;

        // try to place rooms by the number of atempts
        for (int i = 0; i < atempts; i++) {
            // first determine what size the next room will have
            int roomHeight = 0;
            int roomWidth = 0;
            while (roomHeight % 2 == 0)
                roomHeight = Random.Range(minRoomHeight, maxRoomHeight + 1);
            while (roomWidth % 2 == 0)
                roomWidth = Random.Range(minRoomWidth, maxRoomWidth + 1);
            // create a random starttile for the room
            int tileX = 0;
            int tileZ = 0;
            // the tile can be at most at the world height/width minus the room height/width
            // otherwise the room could spawn "outside" the world
            while (tileX % 2 == 0)
                tileX = Random.Range(1, world.Width - roomWidth);
            while (tileZ % 2 == 0)
                tileZ = Random.Range(1, world.Height - roomHeight);
            // now a random valid Tile is selected
            // This Tile will be the "Left Under Tile" for the room
            // then check if the temporary room collides with any other room
            if ( RoomCollidesWithOtherRoom(tileX, tileZ, tileX + roomWidth - 1, tileZ + roomHeight - 1, rooms) == false) {
                rooms.Add(new Room(world.GetTileAt(tileX, tileZ), world.GetTileAt(tileX + roomWidth - 1, tileZ + roomHeight - 1)));
            }
        }
        return rooms;
    }

    // function checks if the given room collides with any other room already placed
    private static bool RoomCollidesWithOtherRoom(int lowerX, int lowerZ, int upperX, int upperZ, List<Room> alreadyValidRooms) {
        // Collision appears when all of these matches:
        // 1. Left side of the roomThatCollides is left of the right side of a room
        // 2. Right side of the roomThatCollides is right of the left side of a room
        // 3. Top side of the roomThatCollides is above the bottom side of a room
        // 4. Bottom side of the roomThatCollides is under of the top side of a room
        // Also i want at least 1 corridor between the rooms
        foreach (Room room in alreadyValidRooms) {
            if (
                //1
                lowerX <= room.upperTile.x + 2 &&
                //2
                upperX >= room.lowerTile.x - 2 &&
                //3
                upperZ >= room.lowerTile.z - 2 &&
                //4
                lowerZ <= room.upperTile.z + 2
            ) {
                return true;
            }
        }
        return false;
    }
}
