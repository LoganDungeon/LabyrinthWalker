using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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

    private void OnEnable() {
        // check if there is (for whatever reason) already an Instance of the WorldController
        if(Instance != null) {
            Debug.LogError("There can only be one Instance of the WorldController!");
        }
        // if not, set the Instance to this WorldController
        Instance = this;

        // check if new World or load from save
        if (OptionsController.NewWorld) {
            // create the new world
            CreateNewWorld();
        }
        else {
            LoadWorld();
        }
        OptionsController.PrintModus();
    }
    
    // creates a new world with the given parameters
    private void CreateNewWorld() {
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

        this.World.CreateRooms();

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
    
    private static void Die() {
        Debug.Log("YOU DIED!!!!");
    }


    #region Saving & Loading
    ////////////////////////
    /// Saving & Loading ///
    ////////////////////////

    public void SaveWorld(  ) {
        // Serializes the World to Xml
        XmlSerializer serializer = new XmlSerializer(typeof(World));
        // For now just one save slot
        FileStream stream = new FileStream(Application.streamingAssetsPath + "/world.xml", FileMode.Create);
        serializer.Serialize(stream, this.World);
        stream.Close();
        Debug.Log("Saved");
    }

    public void LoadWorld(  ) {
        // Deserializes the Xml to a World
        XmlSerializer serializer = new XmlSerializer(typeof(World));
        // Just one SaveSlot
        FileStream stream = new FileStream(Application.streamingAssetsPath + "/world.xml", FileMode.Open);
        this.World = (World)serializer.Deserialize(stream);
        stream.Close();
        Debug.Log("Loaded");
        // The Player itself.
        // Parameters are SpawnTile, (max)health, (max)saturation, (max)stamina
        this.Player = new PlayerCharacter(this.World.GetTileAt(1, 1), 20, 30, 40, 10);
    }
    #endregion
}
