using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextHealth : MonoBehaviour {

    Text text;

    // Use this for initialization
    void Start() {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {

        text.text = "Health:\n" + WorldController.Instance.player.health;
    }
}
