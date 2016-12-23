using UnityEngine;
using System.Collections;

public class World {

    // there can be only one World
    public static World Current {
        get;
        protected set;
    }

    // size of the map
    public int Height {     // x
        get;
        protected set;
    }

    public int Width {      // z
        get;
        protected set;
    }

    // Array of all the tiles in the world
    private readonly Tile[,] _tiles;

    public World( int width, int height ) {
        
        // set the current World to be this world
        Current = this;

        this.Width = width;
        this.Height = height;

        _tiles = new Tile[width, height];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                _tiles[i, j] = new Tile(i, j);
            }
        }
        
    }

    // returns the tile at the specified location
    public Tile GetTileAt(int x, int z) {
        return _tiles[x, z];
    }

    // retuns the tile at the specified location (floored to int)
    public Tile GetTileAt(float x, float z) {
        return _tiles[ Mathf.FloorToInt(x), Mathf.FloorToInt(z)];
    }
}
