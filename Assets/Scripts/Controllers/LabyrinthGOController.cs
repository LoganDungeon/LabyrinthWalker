using UnityEngine;
using System.Collections.Generic;

public class LabyrinthGOController : MonoBehaviour {

    // WallTexture
    // FIXME: Maybe change, that i can choose between different Materials
    public Material wallMaterial;
    public Material floorMaterial;

    // maybe just for develop purposes
    public bool renderAll = false;

    // simple GameObjects which will contain all the Wall/Floor GOs 
    GameObject wallParent;
    GameObject floorParent;

    float wallThickness;

    // World
    World world {
        get {
            return WorldController.Instance.world;
        }
    }

    // Use this for initialization
    void Start() {
        wallThickness = WorldController.Instance.wallThickness;
        // instantiate the parentGameObjects
        wallParent = new GameObject("WallParent");
        floorParent = new GameObject("FloorParent");
        wallParent.transform.SetParent(this.transform);
        floorParent.transform.SetParent(this.transform);
        // go through every Tile and create the GameObjects with the visual Mesh 
        for (int x = 0; x < world.Width; x++) {
            for (int z = 0; z < world.Height; z++) {
                if (world.GetTileAt(x, z).wall == true) {
                    // we are on a wall Tile
                    GameObject wall_go_lower = new GameObject();
                    MeshCollider meshCol = wall_go_lower.AddComponent<MeshCollider>();
                    MeshRenderer renderer = wall_go_lower.AddComponent<MeshRenderer>();
                    MeshFilter mf = wall_go_lower.AddComponent<MeshFilter>();
                    // create the Mesh based on the tiles neighbours
                    Mesh mesh = CreateCubeMesh(wall_go_lower.transform.position, wallThickness, FrontNeighbourIsFloor(x, z), LeftNeighbourIsFloor(x, z), BackNeighbourIsFloor(x, z), RightNeighbourIsFloor(x, z));
                    mf.mesh = mesh;
                    meshCol.sharedMesh = mesh;
                    wall_go_lower.name = "Wall_" + x + "_" + z + "_lower";
                    wall_go_lower.transform.position = new Vector3(wallThickness * x, 0, wallThickness * z);
                    wall_go_lower.transform.SetParent(wallParent.transform);
                    renderer.material = wallMaterial;
                    // Create another Cube on top of the first Cube to create a higher wall
                    GameObject wall_go_upper = Instantiate(wall_go_lower);
                    wall_go_upper.name = "Wall_" + x + "_" + z + "_upper";
                    wall_go_upper.transform.SetParent(wallParent.transform);
                    wall_go_upper.transform.position = new Vector3(wall_go_upper.transform.position.x, wall_go_upper.transform.position.y + wallThickness, wall_go_upper.transform.position.z);

                    wall_go_lower.SetActive(true);
                    wall_go_upper.SetActive(true);

                }
                else { // we are on a floor tile
                    // first create the GameObject and Mesh for the Floor
                    GameObject floor = new GameObject();
                    floor.name = "Floor_" + x + "_" + z;
                    MeshCollider meshCol = floor.AddComponent<MeshCollider>();
                    MeshFilter meshFilter = floor.AddComponent<MeshFilter>();
                    floor.AddComponent<MeshRenderer>();
                    Mesh mesh = CreateFloorMesh(floor.transform.position, wallThickness);
                    meshFilter.mesh = mesh;
                    meshCol.sharedMesh = mesh;
                    floor.transform.SetParent(floorParent.transform);
                    floor.transform.position = new Vector3(wallThickness * x, 0, wallThickness * z);
                    floor.GetComponent<Renderer>().material = floorMaterial;
                    // then create the GameObject and Mesh for the ceiling
                    GameObject ceiling = new GameObject();
                    ceiling.name = "Ceiling_" + x + "_" + z;
                    ceiling.AddComponent<MeshRenderer>();
                    meshCol = ceiling.AddComponent<MeshCollider>();
                    meshFilter = ceiling.AddComponent<MeshFilter>();
                    mesh = CreateCeilingMesh(ceiling.transform.position, wallThickness);
                    meshFilter.mesh = mesh;
                    meshCol.sharedMesh = mesh;
                    ceiling.transform.SetParent(floorParent.transform);
                    ceiling.transform.position = new Vector3(wallThickness * x, 0, wallThickness * z);
                    ceiling.GetComponent<Renderer>().material = floorMaterial;
                }
            }
        }
    }
    
    // Just for developement purposes
    // creates "bubbles" on top of the tiles
    // red colour means floor and blue means wall
    void OnDrawGizmosSelected() {
        if(world == null) {
            return;
        }

        for (int x = 0; x < world.Width; x++) {
            for (int z = 0; z < world.Height; z++) {
                if (world.GetTileAt(x, z).wall == true) {
                    // Create a gizmo sphere, to better analyse the tiles
                    // Wall will be blue
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere( new Vector3((float)(wallThickness*x + 0.5*wallThickness), 2 * wallThickness, (float)(wallThickness * z + 0.5 * wallThickness) ), 0.5f);
                }
                else {
                    // Create a gizmo sphere, to better analyse the tiles
                    // Floor will be red
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(new Vector3((float)(wallThickness * x + 0.5 * wallThickness), 2 * wallThickness, (float)(wallThickness * z + 0.5 * wallThickness)), 0.5f);
                }
            }
        }
    }
    
    void FixedUpdate() {
        // Position of the Player
        Transform playerTransform = WorldController.Instance.player.pcgoC.player_GO.transform;
        
        // only render the tiles, that  are in range of the Character
        // by deactivating the tiles, that are out of range.
        // TODO: i need a better way to do this. i only have to deactivate the visuals of the Objects
        // currently calculated with manhattan distance
        if (!renderAll) {
            foreach (Transform wall in wallParent.transform) {
                if ((Mathf.Abs(wall.transform.position.x - playerTransform.position.x) + Mathf.Abs(wall.transform.position.z - playerTransform.position.z)) < WorldController.Instance.renderDistance) {
                    wall.gameObject.SetActive(true);
                }
                else {
                    wall.gameObject.SetActive(false);
                }
            }

            foreach (Transform floor in floorParent.transform) {
                if ((Mathf.Abs(floor.transform.position.x - playerTransform.position.x) + Mathf.Abs(floor.transform.position.z - playerTransform.position.z)) < WorldController.Instance.renderDistance) {
                    floor.gameObject.SetActive(true);
                }
                else {
                    floor.gameObject.SetActive(false);
                }
            }
        }
    }

    // creates a square floor mesh
    Mesh CreateFloorMesh( Vector3 pos, float length ) {
        return CreateFloorMesh(pos, length, length);
    }

    // creates a square ceiling mesh
    Mesh CreateCeilingMesh( Vector3 pos, float length ) {
        return CreateCeilingMesh(pos, length, length, length);
    }

    // creates a Floor Mesh with the given size
    Mesh CreateFloorMesh( Vector3 pos, float length, float width ) {
        Vector3 p0 = new Vector3(pos.x - 0.5f * length, pos.y, pos.z - 0.5f * width);
        Vector3 p1 = new Vector3(pos.x - 0.5f * length, pos.y, pos.z + 0.5f * width);
        Vector3 p2 = new Vector3(pos.x + 0.5f * length, pos.y, pos.z + 0.5f * width);
        Vector3 p3 = new Vector3(pos.x + 0.5f * length, pos.y, pos.z - 0.5f * width);

        Vector3[] vertices = new Vector3[] {
            p0, p1, p2, p3
        };

        Vector3[] normales = new Vector3[] {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };

        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[] {
            _11, _01, _00, _10
        };

        int[] triangles = new int[] {
            0,1,2,
            0,2,3
        };

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();

        return mesh;
    }

    // creates a Ceiling Mesh with the given size
    Mesh CreateCeilingMesh( Vector3 pos, float length, float width, float height ) {
        Vector3 p0 = new Vector3(pos.x - 0.5f * length, pos.y + 2 * height, pos.z - 0.5f * width);
        Vector3 p1 = new Vector3(pos.x - 0.5f * length, pos.y + 2 * height, pos.z + 0.5f * width);
        Vector3 p2 = new Vector3(pos.x + 0.5f * length, pos.y + 2 * height, pos.z + 0.5f * width);
        Vector3 p3 = new Vector3(pos.x + 0.5f * length, pos.y + 2 * height, pos.z - 0.5f * width);

        Vector3[] vertices = new Vector3[] {
            p0, p1, p2, p3
        };

        Vector3[] normales = new Vector3[] {
            Vector3.down,
            Vector3.down,
            Vector3.down,
            Vector3.down
        };

        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[] {
            _11, _01, _00, _10
        };

        int[] triangles = new int[] {
            2,1,0,
            3,2,0
        };

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();

        return mesh;
    }
    
    // creates a creates a Cube Mesh with the given length
    Mesh CreateCubeMesh( Vector3 pos, float length, bool frontSide, bool leftSide, bool backSide, bool rightSide ) {
        return CreateCuboidMesh( pos, length, length, length, frontSide, leftSide, backSide, rightSide);
    }

    // creates a cuboid Mesh with the given size
    Mesh CreateCuboidMesh( Vector3 pos, float length, float width, float height, bool frontSide, bool leftSide, bool backSide, bool rightSide) {
        #region Vertices
        Vector3 p0 = new Vector3(pos.x - 0.5f * length, pos.y,          pos.z - 0.5f * width);
        Vector3 p1 = new Vector3(pos.x - 0.5f * length, pos.y,          pos.z + 0.5f * width);
        Vector3 p2 = new Vector3(pos.x + 0.5f * length, pos.y,          pos.z + 0.5f * width);
        Vector3 p3 = new Vector3(pos.x + 0.5f * length, pos.y,          pos.z - 0.5f * width);

        Vector3 p4 = new Vector3(pos.x - 0.5f * length, pos.y + height, pos.z - 0.5f * width);
        Vector3 p5 = new Vector3(pos.x - 0.5f * length, pos.y + height, pos.z + 0.5f * width);
        Vector3 p6 = new Vector3(pos.x + 0.5f * length, pos.y + height, pos.z + 0.5f * width);
        Vector3 p7 = new Vector3(pos.x + 0.5f * length, pos.y + height, pos.z - 0.5f * width);

        Vector3[] vertices = new Vector3[] {
	        // Bottom
	        p0, p1, p2, p3,
 
	        // Left
	        p7, p4, p0, p3,
 
	        // Front
	        p4, p5, p1, p0,
 
	        // Back
	        p6, p7, p3, p2,
 
	        // Right
	        p5, p6, p2, p1,
 
	        // Top
	        p7, p6, p5, p4
        };
        #endregion

        #region Normales
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[] {
	        // Bottom
	        down, down, down, down,
 
	        // Left
	        left, left, left, left,
 
	        // Front
	        front, front, front, front,
 
	        // Back
	        back, back, back, back,
 
	        // Right
	        right, right, right, right,
 
	        // Top
	        up, up, up, up
        };
        #endregion

        #region UVs
        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[] {
	        // Bottom
	        _11, _01, _00, _10,
 
	        // Left
	        _11, _01, _00, _10,
 
	        // Front
	        _11, _01, _00, _10,
 
	        // Back
	        _11, _01, _00, _10,
 
	        // Right
	        _11, _01, _00, _10,
 
	        // Top
	        _11, _01, _00, _10,
        };
        #endregion

        #region Triangles
        List<int> triangles = new List<int>();

        if (leftSide) {
            // Left
            triangles.Add(3 + 4 * 1);
            triangles.Add(1 + 4 * 1);
            triangles.Add(0 + 4 * 1);
            triangles.Add(3 + 4 * 1);
            triangles.Add(2 + 4 * 1);
            triangles.Add(1 + 4 * 1);
        }
        if (frontSide) {
            // Front
            triangles.Add(3 + 4 * 2);
            triangles.Add(1 + 4 * 2);
            triangles.Add(0 + 4 * 2);
            triangles.Add(3 + 4 * 2);
            triangles.Add(2 + 4 * 2);
            triangles.Add(1 + 4 * 2);
        }
        if (backSide) {
            // Back
            triangles.Add(3 + 4 * 3);
            triangles.Add(1 + 4 * 3);
            triangles.Add(0 + 4 * 3);
            triangles.Add(3 + 4 * 3);
            triangles.Add(2 + 4 * 3);
            triangles.Add(1 + 4 * 3);
        }
        if (rightSide) {
            // Right
            triangles.Add(3 + 4 * 4);
            triangles.Add(1 + 4 * 4);
            triangles.Add(0 + 4 * 4);
            triangles.Add(3 + 4 * 4);
            triangles.Add(2 + 4 * 4);
            triangles.Add(1 + 4 * 4);
        }
        #endregion

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateBounds();
        mesh.Optimize();

        return mesh;
    }

    // 4 functions used to determine, if the requested neighbour of a tile is Floor or not (wall)
    bool FrontNeighbourIsFloor( int x, int z ) {
        if (x > 0 && world.GetTileAt(x - 1, z).wall == false)
            return true;
        return false;
    }

    bool RightNeighbourIsFloor( int x, int z ) {
        if (z < world.Width - 1 && world.GetTileAt(x, z + 1).wall == false)
            return true;
        return false;
    }

    bool BackNeighbourIsFloor( int x, int z ) {
        if (x < world.Height - 1 && world.GetTileAt(x + 1, z).wall == false)
            return true;
        return false;
    }

    bool LeftNeighbourIsFloor( int x, int z ) {
        if (z > 0 && world.GetTileAt(x, z - 1).wall == false)
            return true;
        return false;
    }

}
