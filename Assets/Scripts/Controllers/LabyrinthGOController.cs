using UnityEngine;
using System.Collections.Generic;

public class LabyrinthGOController : MonoBehaviour
{

    // WallTexture
    // FIXME: Maybe change, that i can choose between different Materials
    public Material WallMaterial;
    public Material FloorMaterial;

    // maybe just for develop purposes
    public bool RenderAll = false;

    // simple GameObjects which will contain all the Wall/Floor GOs 
    private GameObject _wallParent;
    private GameObject _floorParent;

    private float _wallThickness;

    // World
    private static World World
    {
        get
        {
            return WorldController.Instance.World;
        }
    }

    // Use this for initialization
    private void Start()
    {
        _wallThickness = WorldController.Instance.WallThickness;
        // instantiate the parentGameObjects
        _wallParent = new GameObject("WallParent");
        _floorParent = new GameObject("FloorParent");
        _wallParent.transform.SetParent(this.transform);
        _floorParent.transform.SetParent(this.transform);
        // go through every Tile and create the GameObjects with the visual Mesh 
        for(int x = 0; x < World.Width; x++)
        {
            for(int z = 0; z < World.Height; z++)
            {
                if(World.GetTileAt(x, z).IsWall)
                {
                    // we are on a wall Tile
                    GameObject wallGoLower = new GameObject();
                    MeshCollider meshCol = wallGoLower.AddComponent<MeshCollider>();
                    MeshRenderer meshRenderer = wallGoLower.AddComponent<MeshRenderer>();
                    MeshFilter mf = wallGoLower.AddComponent<MeshFilter>();
                    // create the Mesh based on the tiles neighbours
                    Mesh mesh = CreateCubeMesh(wallGoLower.transform.position, _wallThickness,
                        FrontNeighbourIsFloor(x, z), LeftNeighbourIsFloor(x, z), BackNeighbourIsFloor(x, z),
                        RightNeighbourIsFloor(x, z));
                    mf.mesh = mesh;
                    meshCol.sharedMesh = mesh;
                    wallGoLower.name = "Wall_" + x + "_" + z + "_lower";
                    wallGoLower.transform.position = new Vector3(_wallThickness * x, 0, _wallThickness * z);
                    wallGoLower.transform.SetParent(_wallParent.transform);
                    meshRenderer.material = WallMaterial;
                    // Create another Cube on top of the first Cube to create a higher wall
                    GameObject wallGoUpper = Instantiate(wallGoLower);
                    wallGoUpper.name = "Wall_" + x + "_" + z + "_upper";
                    wallGoUpper.transform.SetParent(_wallParent.transform);
                    wallGoUpper.transform.position = new Vector3(wallGoUpper.transform.position.x,
                        wallGoUpper.transform.position.y + _wallThickness, wallGoUpper.transform.position.z);

                    wallGoLower.SetActive(true);
                    wallGoUpper.SetActive(true);

                }
                else
                {
                    // we are on a floor tile
                    // first create the GameObject and Mesh for the Floor
                    GameObject floor = new GameObject
                    {
                        name = "Floor_" + x + "_" + z
                    };
                    MeshCollider meshCol = floor.AddComponent<MeshCollider>();
                    MeshFilter meshFilter = floor.AddComponent<MeshFilter>();
                    floor.AddComponent<MeshRenderer>();
                    Mesh mesh = CreateFloorMesh(floor.transform.position, _wallThickness);
                    meshFilter.mesh = mesh;
                    meshCol.sharedMesh = mesh;
                    floor.transform.SetParent(_floorParent.transform);
                    floor.transform.position = new Vector3(_wallThickness * x, 0, _wallThickness * z);
                    floor.GetComponent<Renderer>().material = FloorMaterial;
                    // then create the GameObject and Mesh for the ceiling
                    GameObject ceiling = new GameObject
                    {
                        name = "Ceiling_" + x + "_" + z
                    };
                    ceiling.AddComponent<MeshRenderer>();
                    meshCol = ceiling.AddComponent<MeshCollider>();
                    meshFilter = ceiling.AddComponent<MeshFilter>();
                    mesh = CreateCeilingMesh(ceiling.transform.position, _wallThickness);
                    meshFilter.mesh = mesh;
                    meshCol.sharedMesh = mesh;
                    ceiling.transform.SetParent(_floorParent.transform);
                    ceiling.transform.position = new Vector3(_wallThickness * x, 0, _wallThickness * z);
                    ceiling.GetComponent<Renderer>().material = FloorMaterial;
                }
            }
        }
    }

    // Just for developement purposes
    // creates "bubbles" on top of the tiles
    // red colour means floor and blue means wall
    //private void OnDrawGizmosSelected() {

    //    if(World == null) {
    //        return;
    //    }

    //    for (int x = 0; x < World.Width; x++) {
    //        for (int z = 0; z < World.Height; z++) {
    //            if (World.GetTileAt(x, z).IsWall) {
    //                // Create a gizmo sphere, to better analyse the tiles
    //                // Wall will be blue
    //                Gizmos.color = Color.blue;
    //                Gizmos.DrawSphere( new Vector3((float)(_wallThickness*x + 0.5*_wallThickness), 2 * _wallThickness, (float)(_wallThickness * z + 0.5 * _wallThickness) ), 0.5f);
    //            }
    //            else {
    //                // Create a gizmo sphere, to better analyse the tiles
    //                // Floor will be red
    //                Gizmos.color = Color.red;
    //                Gizmos.DrawSphere(new Vector3((float)(_wallThickness * x + 0.5 * _wallThickness), 2 * _wallThickness, (float)(_wallThickness * z + 0.5 * _wallThickness)), 0.5f);
    //            }
    //        }
    //    }
    //}

    private void FixedUpdate()
    {

        // Position of the Player
        Transform playerTransform = World.Player.PcgoC.PlayerGo.transform;

        // only render the tiles, that  are in range of the Character
        // by deactivating the tiles, that are out of range.
        // TODO: i need a better way to do this. i only have to deactivate the visuals of the Objects
        // currently calculated with manhattan distance
        if(RenderAll)
            return;
        foreach(Transform wall in _wallParent.transform)
        {
            wall.gameObject.SetActive(Mathf.Abs(wall.transform.position.x - playerTransform.position.x) +
                                      Mathf.Abs(wall.transform.position.z - playerTransform.position.z) <
                                      WorldController.Instance.RenderDistance);
        }

        foreach(Transform floor in _floorParent.transform)
        {
            floor.gameObject.SetActive(Mathf.Abs(floor.transform.position.x - playerTransform.position.x) +
                                       Mathf.Abs(floor.transform.position.z - playerTransform.position.z) <
                                       WorldController.Instance.RenderDistance);
        }
    }

    // creates a square floor mesh
    private static Mesh CreateFloorMesh(Vector3 pos, float length)
    {
        return CreateFloorMesh(pos, length, length);
    }

    // creates a square ceiling mesh
    private Mesh CreateCeilingMesh(Vector3 pos, float length)
    {
        return CreateCeilingMesh(pos, length, length, length);
    }

    // creates a Floor Mesh with the given size
    private static Mesh CreateFloorMesh(Vector3 pos, float length, float width)
    {
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

        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv10 = new Vector2(1f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[] {
            uv11, uv01, uv00, uv10
        };

        int[] triangles = new int[] {
            0,1,2,
            0,2,3
        };

        Mesh mesh = new Mesh
        {
            vertices = vertices,
            normals = normales,
            uv = uvs,
            triangles = triangles
        };
        mesh.RecalculateBounds();
        return mesh;
    }

    // creates a Ceiling Mesh with the given size
    private Mesh CreateCeilingMesh(Vector3 pos, float length, float width, float height)
    {
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

        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv10 = new Vector2(1f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[] {
            uv11, uv01, uv00, uv10
        };

        int[] triangles = new int[] {
            2,1,0,
            3,2,0
        };

        Mesh mesh = new Mesh
        {
            vertices = vertices,
            normals = normales,
            uv = uvs,
            triangles = triangles
        };
        mesh.RecalculateBounds();
        return mesh;
    }

    // creates a creates a Cube Mesh with the given length
    private Mesh CreateCubeMesh(Vector3 pos, float length, bool frontSide, bool leftSide, bool backSide, bool rightSide)
    {
        return CreateCuboidMesh(pos, length, length, length, frontSide, leftSide, backSide, rightSide);
    }

    // creates a cuboid Mesh with the given size
    private Mesh CreateCuboidMesh(Vector3 pos, float length, float width, float height, bool frontSide, bool leftSide, bool backSide, bool rightSide)
    {
        #region Vertices
        Vector3 p0 = new Vector3(pos.x - 0.5f * length, pos.y, pos.z - 0.5f * width);
        Vector3 p1 = new Vector3(pos.x - 0.5f * length, pos.y, pos.z + 0.5f * width);
        Vector3 p2 = new Vector3(pos.x + 0.5f * length, pos.y, pos.z + 0.5f * width);
        Vector3 p3 = new Vector3(pos.x + 0.5f * length, pos.y, pos.z - 0.5f * width);

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
        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv10 = new Vector2(1f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[] {
            // Bottom
            uv11, uv01, uv00, uv10,
 
            // Left
            uv11, uv01, uv00, uv10,
 
            // Front
            uv11, uv01, uv00, uv10,
 
            // Back
            uv11, uv01, uv00, uv10,
 
            // Right
            uv11, uv01, uv00, uv10,
 
            // Top
            uv11, uv01, uv00, uv10,
        };
        #endregion

        #region Triangles
        List<int> triangles = new List<int>();

        if(leftSide)
        {
            // Left
            triangles.Add(3 + 4 * 1);
            triangles.Add(1 + 4 * 1);
            triangles.Add(0 + 4 * 1);
            triangles.Add(3 + 4 * 1);
            triangles.Add(2 + 4 * 1);
            triangles.Add(1 + 4 * 1);
        }
        if(frontSide)
        {
            // Front
            triangles.Add(3 + 4 * 2);
            triangles.Add(1 + 4 * 2);
            triangles.Add(0 + 4 * 2);
            triangles.Add(3 + 4 * 2);
            triangles.Add(2 + 4 * 2);
            triangles.Add(1 + 4 * 2);
        }
        if(backSide)
        {
            // Back
            triangles.Add(3 + 4 * 3);
            triangles.Add(1 + 4 * 3);
            triangles.Add(0 + 4 * 3);
            triangles.Add(3 + 4 * 3);
            triangles.Add(2 + 4 * 3);
            triangles.Add(1 + 4 * 3);
        }
        if(rightSide)
        {
            // Right
            triangles.Add(3 + 4 * 4);
            triangles.Add(1 + 4 * 4);
            triangles.Add(0 + 4 * 4);
            triangles.Add(3 + 4 * 4);
            triangles.Add(2 + 4 * 4);
            triangles.Add(1 + 4 * 4);
        }
        #endregion

        Mesh mesh = new Mesh
        {
            vertices = vertices,
            normals = normales,
            uv = uvs,
            triangles = triangles.ToArray()
        };
        mesh.RecalculateBounds();
        return mesh;
    }

    // 4 functions used to determine, if the requested neighbour of a tile is Floor or not (wall)
    private bool FrontNeighbourIsFloor(int x, int z)
    {
        return !(x > 0 && World.GetTileAt(x - 1, z).IsWall);
    }

    private bool RightNeighbourIsFloor(int x, int z)
    {
        return !(z < World.Width - 1 && World.GetTileAt(x, z + 1).IsWall);
    }

    private bool BackNeighbourIsFloor(int x, int z)
    {
        return !(x < World.Height - 1 && World.GetTileAt(x + 1, z).IsWall);
    }

    private bool LeftNeighbourIsFloor(int x, int z)
    {
        return !(z > 0 && World.GetTileAt(x, z - 1).IsWall);
    }
}
