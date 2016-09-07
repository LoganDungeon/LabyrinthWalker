using UnityEngine;
using System.Collections;

public class PlayerCharacterGOController : MonoBehaviour {

    public GameObject player_GO;

    void Start() {

    }

    void Update() {
        
    }

    void FixedUpdate() {

    }

    public void CreateCharacterGameObject(Tile t) {
        Debug.Log("Created CharacterGameObject");
        player_GO = Instantiate(WorldController.Instance.character.gameObject);
        player_GO.transform.position = new Vector3(t.x * WorldController.Instance.wallThickness, 1.5f, t.z * WorldController.Instance.wallThickness);
        player_GO.transform.SetParent(this.transform);
    }
}
