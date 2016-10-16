using UnityEngine;
using System.Collections;

public class Tile {

    // this class is a pure data class
    // it contains various informations about 1 Tile, or "cell" of the 
    // planned Labyrinth

    // declares, if there will be generated a wall on this tile
    public bool wall;

    // declares if the tile was already visited by the depth first search algorithm
    public bool visited;

    // asd
    public bool isRoomWall;

    // Coordinates;
    public int x;
    public int z;

    public Tile( int x, int z ) {
        this.x = x;
        this.z = z;
        this.wall = true;
        this.visited = false;
        this.isRoomWall = false;

        //Debug.Log("Tile_" + x + "_" + z + " created");
    }
}
