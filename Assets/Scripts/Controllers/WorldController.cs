using UnityEngine;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

    // The Character
    public GameObject PlayerCharacter;

    // Size of the world
    public int Width;
    public int Height;

    // set the RenderDistance
    // TODO: Create an Options Menu
    public int RenderDistance;

    // Thickness of the walls
    public float WallThickness;

    // the Instance ensures, that there is only ONE worldController
    public static WorldController Instance {
        get;
        private set;
    }

    public World World {
        get;
        private set;
    }

    public PlayerCharacter Player {
        get;
        private set;
    }

    public List<Character> Npcs {
        get;
        set;
    }

    public List<Room> Rooms {
        get;
        private set;
    }

    private void OnEnable() {
        // cheack if there is (for whatever reason) already an Instance of the WorldController
        if(Instance != null) {
            Debug.LogError("There can only be one Instance of the WorldController!");
        }
        // if not, set the Instance to this WorldController
        Instance = this;

        // then create the world
        CreateWorld();
    }

    // creates the world with the given parameters
    private void CreateWorld() {
        // check if the wallthickness is greater than 0
        if (WallThickness <= 0)
            WallThickness = 1;
        // check if height and width are positive
        if (Height < 0)
            Height = Mathf.Abs(Height);
        if (Width < 0)
            Width = Mathf.Abs(Width);
        // check if height and width are odd numbers.
        // if not, add one.
        if (Height % 2 == 0) {
            Height++;
        }
        if (Width % 2 == 0) {
            Width++;
        }
        // check if height and width are at least 5
        if (Height < 5) {
            Height = 5;
        }
        if (Width < 5) {
            Width = 5;
        }
        // Create the World with the given height and width
        this.World = new World(Width, Height);
        
        // create the rooms
        this.Rooms = new List<Room>();
        this.Rooms = MapGenerator.CreateRooms();

        // Create the Maze
        MapGenerator.CreateMaze();

        // The Player itself.
        // Parameters are SpawnTile, (max)health, (max)saturation, (max)stamina
        this.Player = new PlayerCharacter(this.World.GetTileAt(1, 1), 20, 30, 40, 10);
    }

    private void Update() {
        // JUST FOR TESTING PURPOSES
        if (Input.GetKeyDown(KeyCode.X)) {
            this.Player.Health--;
        }

        if (this.Player.Health <= 0) {
            Die();
        }
    }
    
    private void Die() {
        Debug.Log("YOU DIED!!!!");
    }
}
