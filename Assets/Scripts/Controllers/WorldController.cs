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

    // ensures, that there is only ONE worldController
    public static WorldController Instance { get; protected set; }
    public        World           world    { get; protected set; }
    public        PlayerCharacter player   { get; protected set; }
    public        List<Character> npcs     { get; protected set; }

    void OnEnable() {
        if(Instance != null) {
            Debug.LogError("There can only be one Instance of the WorldController!");
        }
        Instance = this;

        CreateWorld();
    }

    void CreateWorld() {
        // check if the wallthickness is greater than 0
        if (wallThickness <= 0)
            wallThickness = 1;
        // check if height and width are positive
        if (height < 0) {
            height = Mathf.Abs(height);
            //Debug.Log("Height made positive: " + height);
        }
        if (width < 0) {
            width = Mathf.Abs(width);
            //Debug.Log("Width made positive: " + width);
        }
        // check if height and width are odd numbers.
        // if not, add one.
        if (height % 2 == 0) {
            height++;
            //Debug.Log("Height increased: " + height);
        }
        if (width % 2 == 0) {
            width++;
            //Debug.Log("Width increased: " + width);
        }
        // check if height and width are at least 5
        if (height < 5) {
            height = 5;
            //Debug.Log("Height minimum: " + height);
        }
        if (width < 5) {
            width = 5;
            //Debug.Log("Width minimum: " + width);
        }
        // Create the World with the given height and width
        world = new World(width, height);
        //Debug.Log("WorldController: World was created");

        MazeGenerator mg = new MazeGenerator();
        mg.CreateMaze();

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

    public void CreateCharacter( Tile t ) {
                GameObject ch = MonoBehaviour.Instantiate(character);
                ch.transform.position = new Vector3(t.x + (float)wallThickness / 2, 1, t.z + (float)wallThickness / 2);
                Debug.Log("Character visuals created at: "+ ch.transform.position.x + "_" + ch.transform.position.z);
    }

    void Die() {
        Debug.Log("YOU DIED!!!!");
    }

}
