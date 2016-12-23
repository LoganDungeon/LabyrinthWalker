using UnityEngine;
using System.Collections;

public class Tile {

    // this class is a pure data class
    // it contains various informations about 1 Tile, or "cell" of the 
    // planned Labyrinth

    // declares, if there will be generated a wall on this tile
    public bool IsWall {
        get;
        set;
    }

    // declares if the tile was already visited by the depth first search algorithm
    public bool Visited {
        get;
        set;
    }

    public bool IsRoomWall {
        get;
        set;
    }

    // Coordinates;
    public int X {
        get;
        set;
    }

    public int Z {
        get;
        set;
    }

    public Tile( int x, int z ) {
        this.X = x;
        this.Z = z;
        this.Visited = false;
        this.IsWall = true;
        this.IsRoomWall = false;
    }
}
