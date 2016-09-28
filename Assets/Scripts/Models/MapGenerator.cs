////////////////////////////////////////////////////////////////////////////////////
/// Algorithm comes from https://en.wikipedia.org/wiki/Maze_generation_algorithm ///
/// Depth First Search Recursive Backtracker                                     ///
////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections.Generic;

public static class MapGenerator {

    private static World world = WorldController.Instance.world;

    public static void CreateMaze() {
        Tile currentTile;
        Tile nextTile;
        Stack<Tile> visitedTiles = new Stack<Tile>();
        int randX = Random.Range(1, world.Width - 1);
        int randZ = Random.Range(1, world.Height - 1);
        // first i will get a random tile to start from
        while (randX % 2 == 0) {
            randX = Random.Range(1, world.Width - 1);
        }
        while (randZ % 2 == 0) {
            randZ = Random.Range(1, world.Height - 1);
        }

        //Debug.Log("Starttile: " + randX + "_" + randZ);

        //===== FIRST STEP =====//
        // set the starting tile
        currentTile = world.GetTileAt(randX, randZ);
        // mark the startingTile as visited
        currentTile.visited = true;
        currentTile.wall = false;
        //currentTile = world.GetTileAt(3, 3);

        //world.GetTileAt(1, 3).visited = true;
        //world.GetTileAt(3, 1).visited = true;
        //world.GetTileAt(5, 3).visited = true;
        //world.GetTileAt(3, 5).visited = true;

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

    static List<Tile> GetNeighbours(Tile tile) {
        List<Tile> neighbours = new List<Tile>();
        // add the north tile
        if (tile.z < world.Height - 3 && world.GetTileAt(tile.x, tile.z + 2).visited == false) {
            neighbours.Add(world.GetTileAt(tile.x, tile.z + 2));
        }
        // add the east tile
        if (tile.x < world.Width - 3 && world.GetTileAt(tile.x + 2, tile.z).visited == false) {
            neighbours.Add(world.GetTileAt(tile.x + 2, tile.z));
        }
        // add the south tile
        if (tile.z >= 3 && world.GetTileAt(tile.x, tile.z - 2).visited == false) {
            neighbours.Add(world.GetTileAt(tile.x, tile.z - 2));
        }
        // add the west tile
        if (tile.x >= 3 && world.GetTileAt(tile.x - 2, tile.z).visited == false) {
            neighbours.Add(world.GetTileAt(tile.x - 2, tile.z));
        }
        return neighbours;
    }

    static Tile GetMiddleTile( Tile currentTile, Tile nextTile ) {
        return WorldController.Instance.world.GetTileAt( (currentTile.x + nextTile.x)/2, (currentTile.z + nextTile.z)/2 );
    }

    public static List<Room> CreateRooms() {

        List<Room> rooms = new List<Room>();

        // number of the atempts, how often the algorithm will try to place valid rooms
        int atempts = Mathf.FloorToInt(Mathf.Sqrt(WorldController.Instance.world.Height * WorldController.Instance.world.Width));

        // size of the rooms will be between 3 and a certain calculated value based on the world width/height
        int minRoomHeight = 3;
        int minRoomWidth= 3;
        int maxRoomHeight = (Mathf.FloorToInt(Mathf.Pow(Mathf.Pow(world.Height, 2f), 1 / 3f) - (Mathf.Sqrt((Mathf.Pow(Mathf.Pow(world.Height, 2f), 1 / 3f))))));
        int maxRoomWidth  = (Mathf.FloorToInt(Mathf.Pow(Mathf.Pow(world.Width, 2f), 1 / 3f) - (Mathf.Sqrt((Mathf.Pow(Mathf.Pow(world.Width, 2f), 1 / 3f)))))); ;

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
            while (tileX % 2 == 0)
                tileX = Random.Range(0, world.Height);
            while (tileZ % 2 == 0)
                tileZ = Random.Range(0, world.Width);
            // now we have a valid startTile
        }
        

        //rooms.Add(new Room(
        //        world.GetTileAt(tileX, tileZ),
        //        world.GetTileAt(tileX + Random.Range(minRoomHeight, maxRoomHeight + 1), tileZ + Random.Range(minRoomWidth, maxRoomWidth + 1))
        //    ));

        return rooms;
    }
}
