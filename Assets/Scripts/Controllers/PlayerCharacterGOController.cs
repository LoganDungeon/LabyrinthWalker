using UnityEngine;

// will handle the GameObject with the visual side of the PlayerCharacter
public class PlayerCharacterGOController : MonoBehaviour
{

    // the actual GameObject of the Player
    public GameObject PlayerGo
    {
        get;
        set;
    }
    /*
    void Start() {

    }

    void Update() {
        
    }

    void FixedUpdate() {

    }
    */
    // creates the PlayerCharacter at the position of the given tile
    public void CreateCharacterGameObject(Tile t)
    {
        Debug.Log("Created CharacterGameObject");
        this.PlayerGo = Instantiate(WorldController.Instance.PlayerCharacter.gameObject);
        this.PlayerGo.transform.position = new Vector3(t.X * WorldController.Instance.WallThickness, 1.5f, t.Z * WorldController.Instance.WallThickness);
        this.PlayerGo.transform.SetParent(this.transform);
    }
}
