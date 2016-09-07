using UnityEngine;
using System.Collections;

public class LabyrinthGOController : MonoBehaviour {

    // WallTexture
    // FIXME: Maybe change, that i can choose between different Materials
    public Material wallMaterial;
    public Material floorMaterial;

    GameObject wallParent;
    GameObject floorParent;

    GameObject character;

    // World
    World world {
        get {
            return WorldController.Instance.world;
        }
    }

    

    // Use this for initialization
    void Start() {
        

        character = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log("Character created at: " + character.transform.position.x.ToString() + "_" + character.transform.position.z.ToString() );

        wallParent = new GameObject("WallParent");
        floorParent = new GameObject("FloorParent");

        wallParent.transform.SetParent(this.transform);
        floorParent.transform.SetParent(this.transform);

        for (int x = 0; x < world.Width; x++) {
            for (int z = 0; z < world.Height; z++) {
                if (world.GetTileAt(x, z).wall == true) {
                    // Create a GO, set name, position, order them in to their parent, set the material
                    GameObject wall_go_lower = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall_go_lower.name = "Wall_" + x + "_" + z + "_lower";
                    wall_go_lower.transform.position = new Vector3(WorldController.Instance.wallThickness * x, 1, WorldController.Instance.wallThickness * z);
                    wall_go_lower.transform.localScale = new Vector3(WorldController.Instance.wallThickness, 1, WorldController.Instance.wallThickness);
                    wall_go_lower.transform.SetParent(wallParent.transform);
                    wall_go_lower.GetComponent<Renderer>().material = wallMaterial;
                    // Create another Cube on top of the first Cube to create a higher wall
                    GameObject wall_go_upper = Instantiate(wall_go_lower);
                    wall_go_upper.name = "Wall_" + x + "_" + z + "_upper";
                    wall_go_upper.transform.SetParent(wallParent.transform);
                    wall_go_upper.transform.position = new Vector3(wall_go_upper.transform.position.x, wall_go_upper.transform.position.y + 1, wall_go_upper.transform.position.z);

                    wall_go_lower.SetActive(true);
                    wall_go_upper.SetActive(true);

                }
                else {
                    GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    floor.transform.SetParent(floorParent.transform);
                    floor.transform.localScale = new Vector3((float)WorldController.Instance.wallThickness/10, 1, (float)WorldController.Instance.wallThickness /10);
                    floor.transform.position = new Vector3(WorldController.Instance.wallThickness * x, 0.5f, WorldController.Instance.wallThickness * z);
                    floor.GetComponent<Renderer>().material = floorMaterial;
                }
            }
        }
    }

    void asdFixedUpdate() {
        //Debug.Log("FixedUpdate");

        foreach (Transform wall in wallParent.transform) {
            if ((Mathf.Abs(wall.transform.position.x - character.transform.position.x) + Mathf.Abs(wall.transform.position.z - character.transform.position.z)) < WorldController.Instance.renderDistance) {
                //wall.gameObject.GetComponent<MeshRenderer>().enabled = true;
                wall.gameObject.SetActive(true);
            }
            else {
                //wall.gameObject.GetComponent<MeshRenderer>().enabled = false;
                wall.gameObject.SetActive(false);
                //wall.gameObject.SetActive(true);
            }
        }

        foreach (Transform floor in floorParent.transform) {
            if ((Mathf.Abs(floor.transform.position.x - character.transform.position.x) + Mathf.Abs(floor.transform.position.z - character.transform.position.z)) < WorldController.Instance.renderDistance) {
                //floor.gameObject.GetComponent<MeshRenderer>().enabled = true;
                floor.gameObject.SetActive(true);
                Debug.Log("Test");
            }
            else {
                //floor.gameObject.GetComponent<MeshRenderer>().enabled = false;
                floor.gameObject.SetActive(false);
                //floor.gameObject.SetActive(true);
            }
        }
    }
}
