using UnityEngine;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

    // The Character
    public GameObject character;

    // Size of the world
    public int width;
    public int height;

    // set the RenderDistance
    // TODO: Create an Options Menu
    public int renderDistance;

    // Thickness of the walls
    public float wallThickness;

    // the Instance ensures, that there is only ONE worldController
    public static WorldController Instance { get; protected set; }
    public        World           world    { get; protected set; }
    public        PlayerCharacter player   { get; protected set; }
    public        List<Character> npcs     { get; protected set; }
    public        List<Room>      rooms    { get; protected set; }

    void OnEnable() {
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
    void CreateWorld() {
        // check if the wallthickness is greater than 0
        if (wallThickness <= 0)
            wallThickness = 1;
        // check if height and width are positive
        if (height < 0) {
            height = Mathf.Abs(height);
        }
        if (width < 0) {
            width = Mathf.Abs(width);
        }
        // check if height and width are odd numbers.
        // if not, add one.
        if (height % 2 == 0) {
            height++;
        }
        if (width % 2 == 0) {
            width++;
        }
        // check if height and width are at least 5
        if (height < 5) {
            height = 5;
        }
        if (width < 5) {
            width = 5;
        }
        // Create the World with the given height and width
        world = new World(width, height);
        
        // create the rooms
        rooms = new List<Room>();
        rooms = MapGenerator.CreateRooms();

        // Create the Maze
        MapGenerator.CreateMaze();

        // The Player itself.
        // Parameters are SpawnTile, (max)health, (max)saturation, (max)stamina
        player = new PlayerCharacter(world.GetTileAt(1, 1), 20, 30, 40, 10);
    }

    void Update() {
        // JUST FOR TESTING PURPOSES
        if (Input.GetKeyDown(KeyCode.X)) {
            player.health--;
        }

        if (player.health <= 0) {
            Die();
        }
    }
    
    void Die() {
        Debug.Log("YOU DIED!!!!");
    }

}
