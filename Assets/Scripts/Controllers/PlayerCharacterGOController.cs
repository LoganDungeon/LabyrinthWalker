using UnityEngine;
using System.Collections;

// will handle the GameObject with the visual side of the PlayerCharacter
public class PlayerCharacterGOController : MonoBehaviour {

    // the actual GameObject of the Player
    public GameObject player_GO;

    void Start() {

    }

    void Update() {
        
    }

    void FixedUpdate() {

    }

    // creates the PlayerCharacter at the position of the given tile
    public void CreateCharacterGameObject(Tile t) {
        Debug.Log("Created CharacterGameObject");
        player_GO = Instantiate(WorldController.Instance.character.gameObject);
        player_GO.transform.position = new Vector3(t.x * WorldController.Instance.wallThickness, 1.5f, t.z * WorldController.Instance.wallThickness);
        player_GO.transform.SetParent(this.transform);
    }
}
