using UnityEngine;
using System.Collections;

public class World2 : MonoBehaviour {

    public static World2 currentWorld;

    public int chunkWidth = 100, chunkHeight = 100, seed = 0;

    // Use this for initialization
    void Awake() {
        currentWorld = this;
        if (seed == 0)
            seed = Random.Range(0, int.MaxValue);
    }

    // Update is called once per frame
    void Update() {

    }
}
