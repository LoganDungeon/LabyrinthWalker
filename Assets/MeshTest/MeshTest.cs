using UnityEngine;
using System.Collections.Generic;

public class MeshTest : MonoBehaviour {

    public Material wallmat;

    void Start() {
        CreateCube(new Vector3(9, 0, 9), 1, 1, 1);
        CreateCube(new Vector3(5, 0, 5), 2, 2, 2);
        CreateCube(new Vector3(1, 1, 1), 3, 3, 3);
        CreateCube(new Vector3(-10, 0, -10), 10, 10, 10);
        CreateCube(new Vector3(10, 0, -10), 10, 10, 50);
    }


    void CreateCube( Vector3 pos, float length, float width, float height ) {
        GameObject go = new GameObject();
        go.transform.position = new Vector3();
        // You can change that line to provide another MeshFilter
        MeshFilter filter = go.AddComponent<MeshFilter>();
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        Mesh mesh = filter.mesh;
        renderer.material = wallmat;
        mesh.Clear();


        #region Vertices
        /*
        Vector3 p0 = pos;
        Vector3 p1 = new Vector3(pos.x, pos.y, pos.z + length);
        Vector3 p2 = new Vector3(pos.x + length, pos.y, pos.z + length);
        Vector3 p3 = new Vector3(pos.x + length, pos.y, pos.z);

        Vector3 p4 = new Vector3(pos.x, pos.y + length, pos.z);
        Vector3 p5 = new Vector3(pos.x, pos.y + length, pos.z + length);
        Vector3 p6 = new Vector3(pos.x + length, pos.y + length, pos.z + length);
        Vector3 p7 = new Vector3(pos.x + length, pos.y + length, pos.z);
        */

        Vector3 p0 = pos;
        Vector3 p1 = new Vector3(pos.x, pos.y, pos.z + width);
        Vector3 p2 = new Vector3(pos.x + length, pos.y, pos.z + width);
        Vector3 p3 = new Vector3(pos.x + length, pos.y, pos.z);

        Vector3 p4 = new Vector3(pos.x, pos.y + height, pos.z);
        Vector3 p5 = new Vector3(pos.x, pos.y + height, pos.z + width);
        Vector3 p6 = new Vector3(pos.x + length, pos.y + height, pos.z + width);
        Vector3 p7 = new Vector3(pos.x + length, pos.y + height, pos.z);


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
        int[] triangles = new int[] {
            // Bottom
            3, 1, 0,
            3, 2, 1,			

	        // Left
	        3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
            3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
 
	        // Front
	        3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
            3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
 
	        // Back
	        3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
            3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
 
	        // Right
	        3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
            3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
 
	        // Top
	        3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
            3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,

        };
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    public virtual void CreateVisualMesh() {
        GameObject minecraftMechMethod = new GameObject();

        MeshRenderer meshRenderer = minecraftMechMethod.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = minecraftMechMethod.AddComponent<MeshCollider>();
        MeshFilter meshFilter = minecraftMechMethod.AddComponent<MeshFilter>();
        Mesh visualMesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        

        visualMesh.vertices = verts.ToArray();
        visualMesh.uv = uvs.ToArray();
        visualMesh.triangles = tris.ToArray();
        visualMesh.RecalculateBounds();
        visualMesh.RecalculateNormals();

        meshFilter.mesh = visualMesh;
        meshCollider.sharedMesh = visualMesh;

    }

    public virtual void BuildFace( Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris ) {
        int index = verts.Count;

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));

        if (reversed) {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 0);
        }
        else {
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 2);
            tris.Add(index + 0);
        }
    }
}
