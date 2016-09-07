using UnityEngine;
using System.Collections;

public class PlayerCharacterGOController : MonoBehaviour {

    public GameObject player_GO;

    public PlayerCharacterGOController() {
        Debug.Log("PlayerCharacterGOController");
        player_GO = Instantiate(WorldController.Instance.character);
    }


    void Update() {
        //player_GO.transform.position = WorldController.Instance.player.position;
        Debug.Log("BlaBlaBla");
    }

    void FixedUpdate() {

    }
}
