using UnityEngine;
using System.Collections.Generic;

// static class which will contain all Generating functions
public static class MapGenerator
{

    // reference to the world
    private static readonly World World = WorldController.Instance.World;

    ////////////////////////////////////////////////////////////////////////////////////
    /// Algorithm comes from https://en.wikipedia.org/wiki/Maze_generation_algorithm ///
    /// Depth First Search Recursive Backtracker                                     ///
    ////////////////////////////////////////////////////////////////////////////////////
    public static void CreateMaze()
    {
        Stack<Tile> visitedTiles = new Stack<Tile>();
        int randX = Random.Range(1, World.Width - 1);
        int randZ = Random.Range(1, World.Height - 1);
        // first i will get a random tile to start from, which has not been "visited" yet (outside of a room)
        do
        {
            while(randX % 2 == 0)
            {
                randX = Random.Range(1, World.Width - 1);
            }
            while(randZ % 2 == 0)
            {
                randZ = Random.Range(1, World.Height - 1);
            }
        }
        while(World.GetTileAt(randX, randZ).Visited);

        //===== FIRST STEP =====//
        // set the starting tile
        Tile currentTile = World.GetTileAt(randX, randZ);
        // mark the startingTile as visited
        currentTile.Visited = true;
        currentTile.IsWall = false;

        //===== SECOND STEP =====//
        // while there are tiles with visited == false
        while(AreThereUnvisitedTiles())
        {
            //=== First Substep ===//
            // first get all the neighbours of the tile, that has not been visited yet
            List<Tile> neighbours = GetNeighbours(currentTile);
            // Check if the current Tile has any unvisited neighbours
            if(neighbours.Count > 0)
            {
                //=1=//
                // chose a random Neighbour
                Tile nextTile = neighbours[Random.Range(1, 1000) % (neighbours.Count)];
                //=2=//
                // add the current Tile to the stack
                visitedTiles.Push(nextTile);
                //=3=//
                // remove the wall between the currentTile and the nextTile
                GetMiddleTile(currentTile, nextTile).IsWall = false;
                nextTile.IsWall = false;
                //=4=//
                // go to the nextTile and mark it as visited
                currentTile = nextTile;
                currentTile.Visited = true;
            }
            // there are no unvisited neighbours and we go back
            else if(visitedTiles.Count > 0)
            {
                currentTile = visitedTiles.Pop();
            }
        }
    }

    private static bool AreThereUnvisitedTiles()
    {
        // go through all of the tiles and return true, if there is at least one unvisited tile
        for(int i = 1; i < World.Width; i += 2)
        {
            for(int j = 1; j < World.Height; j += 2)
            {
                if(World.GetTileAt(i, j).Visited == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // returns a List with all neighbours, that has not been visited yet
    private static List<Tile> GetNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();
        // add the north tile
        if(tile.Z < World.Height - 3 && World.GetTileAt(tile.X, tile.Z + 2).Visited == false && GetMiddleTile(tile, World.GetTileAt(tile.X, tile.Z + 2)).IsRoomWall == false)
        {
            neighbours.Add(World.GetTileAt(tile.X, tile.Z + 2));
        }
        // add the east tile
        if(tile.X < World.Width - 3 && World.GetTileAt(tile.X + 2, tile.Z).Visited == false && GetMiddleTile(tile, World.GetTileAt(tile.X + 2, tile.Z)).IsRoomWall == false)
        {
            neighbours.Add(World.GetTileAt(tile.X + 2, tile.Z));
        }
        // add the south tile
        if(tile.Z >= 3 && World.GetTileAt(tile.X, tile.Z - 2).Visited == false && GetMiddleTile(tile, World.GetTileAt(tile.X, tile.Z - 2)).IsRoomWall == false)
        {
            neighbours.Add(World.GetTileAt(tile.X, tile.Z - 2));
        }
        // add the west tile
        if(tile.X >= 3 && World.GetTileAt(tile.X - 2, tile.Z).Visited == false && GetMiddleTile(tile, World.GetTileAt(tile.X - 2, tile.Z)).IsRoomWall == false)
        {
            neighbours.Add(World.GetTileAt(tile.X - 2, tile.Z));
        }
        return neighbours;
    }

    // returns the tile inbetween of the given two tiles
    private static Tile GetMiddleTile(Tile currentTile, Tile nextTile)
    {
        return WorldController.Instance.World.GetTileAt((currentTile.X + nextTile.X) / 2, (currentTile.Z + nextTile.Z) / 2);
    }
}
