using UnityEngine;
using System.Collections;

public class World {

    // there can be only one World
    public static World current {
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
    Tile[,] tiles;

    public World( int width, int height ) {
        
        // set the current World to be this world
        current = this;

        this.Width = width;
        this.Height = height;

        this.tiles = new Tile[width, height];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                tiles[i, j] = new Tile(i, j);
            }
        }
        
    }

    public Tile GetTileAt(int x, int z) {
        return tiles[x, z];
    }

    public Tile GetTileAt(float x, float z) {
        return tiles[ Mathf.FloorToInt(x), Mathf.FloorToInt(z)];
    }
}
