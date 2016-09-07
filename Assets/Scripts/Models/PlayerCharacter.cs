using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

    // Inventory of the Player
    public Inventory playerInventory;

    // saturation will go down with the time and stamina will be used to sprint
    public int saturation;
    public int stamina;

    // visuals of the Character
    PlayerCharacterGOController pcgoC;

    public PlayerCharacter(Tile tile, int _health, int _saturation, int _stamina): base(tile, _health) {

        pcgoC = new PlayerCharacterGOController();
        pcgoC.player_GO.transform.position = new Vector3( tile.x * WorldController.Instance.wallThickness, 1.5f, tile.z * WorldController.Instance.wallThickness);

        saturation = _saturation;
        stamina = _stamina;

        playerInventory = new Inventory(10);

//        GameObject ch = MonoBehaviour.Instantiate(character);
//        ch.transform.position = new Vector3(t.x + (float)wallThickness / 2, 1, t.z + (float)wallThickness / 2);
//        Debug.Log(ch.transform.position.x + "_" + ch.transform.position.z);
    }

}
